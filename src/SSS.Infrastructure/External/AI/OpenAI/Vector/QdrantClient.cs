using SSS.Application.Abstractions.External.AI.Vector;
using SSS.Domain.Entities.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SSS.Infrastructure.External.AI.OpenAI.Vector
{
    public class QdrantClient : IQdrantClient
    {
        private readonly HttpClient _client;
        private readonly AiOptions _cfg;
        private string? _activeCollection;
        public QdrantClient(IHttpClientFactory client, AiOptions cfg)
        {

            _cfg = cfg;
            _client = client.CreateClient(nameof(QdrantClient));
            _client.BaseAddress = new Uri(_cfg.Qdrant.Endpoint.Trim('/') + "/");
        }

        private static async Task EnsureSuccessWithBodyAsync(HttpResponseMessage res, CancellationToken ct)
        {
            if (res.IsSuccessStatusCode) return;

            var body = await res.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException(
                $"Qdrant request failed. Status={(int)res.StatusCode} ({res.ReasonPhrase}). Body={body}",
                null,
                res.StatusCode);
        }

        private async Task EnsurePayloadIndexAsync(string collectionName, string fieldName, object fieldSchema, CancellationToken ct)
        {
            var body = new
            {
                field_name = fieldName,
                field_schema = fieldSchema
            };

            using var res = await _client.PutAsJsonAsync($"collections/{collectionName}/index", body, ct);
            if (res.IsSuccessStatusCode) return;

            // If the index already exists, treat it as success.
            if (res.StatusCode == HttpStatusCode.Conflict) return;

            // Some Qdrant versions may return 400 with an "already exists" message.
            if (res.StatusCode == HttpStatusCode.BadRequest)
            {
                var text = await res.Content.ReadAsStringAsync(ct);
                if (text.Contains("already exists", StringComparison.OrdinalIgnoreCase)) return;
            }

            res.EnsureSuccessStatusCode();
        }

        private async Task EnsureBehaviorQueryIndexesAsync(string collectionName, CancellationToken ct)
        {
            await EnsurePayloadIndexAsync(collectionName, fieldName: "user_id", fieldSchema: "uuid", ct);
            await EnsurePayloadIndexAsync(collectionName, fieldName: "studyplan_id", fieldSchema: "keyword", ct);
            await EnsurePayloadIndexAsync(collectionName, fieldName: "data_type", fieldSchema: "keyword", ct);
            await EnsurePayloadIndexAsync(collectionName, fieldName: "created_at", fieldSchema: "datetime", ct);
        }

        public async Task EnsureCollectionAsync(int vectorSize, CancellationToken ct = default)
        {
            var baseName = _cfg.Qdrant.Collection;
            var name = baseName.EndsWith($"_{vectorSize}")
                ? baseName
                : $"{baseName}_{vectorSize}";
            _activeCollection = name;
            // check exists
            var check = await _client.GetAsync($"collections/{name}", ct);
            if (!check.IsSuccessStatusCode)
            {
                var body = new
                {
                    vectors = new { size = vectorSize, distance = "Cosine" }

                };

                var resp = await _client.PutAsJsonAsync($"collections/{name}", body, ct);
                resp.EnsureSuccessStatusCode();
            }

            // Required for filtered searches and latest behavior sorting.
            await EnsureBehaviorQueryIndexesAsync(name, ct);
        }

        public async Task<List<VecHit>> SearchAsync(float[] query, int topK, CancellationToken ct = default)
        {
            await EnsureCollectionAsync(query.Length, ct);

            var name = _activeCollection ?? _cfg.Qdrant.Collection;
            var body = new
            {
                vector = query,
                limit = topK,
                with_payload = true
            };

            var res = await _client.PostAsJsonAsync($"collections/{name}/points/search", body, ct);
            await EnsureSuccessWithBodyAsync(res, ct);
            var json = await res.Content.ReadFromJsonAsync<QdrantSearchResponse>(cancellationToken: ct);

            var result = new List<VecHit>();
            foreach (var item in json!.Result)
            {
                result.Add(new VecHit(item.Id, item.Score, item.Payload.Text, item.Payload.Source));
            }
            return result;
        }

        public async Task<List<VecHit>> SearchByUserId(float[] vector, int topK, string userId, string studyplanId, string dataType, CancellationToken ct = default)
        {

            await EnsureCollectionAsync(vector.Length, ct);

            var name = _activeCollection ?? _cfg.Qdrant.Collection;

            var body = new
            {
                vector,
                limit = topK,
                with_payload = true,
                filter = new
                {
                    must = new object[]
                    {
                new
                {
                    key = "user_id",
                    match = new { value = userId }
                },
                 new
                {
                    key = "studyplan_id",
                    match = new { value = studyplanId }
                },
                new
                {
                    key = "data_type",
                    match = new { value = dataType }
                }
                    }
                }
            };

            var res = await _client.PostAsJsonAsync(
                $"collections/{name}/points/search",
                body,
                ct);


            await EnsureSuccessWithBodyAsync(res, ct);

            var json = await res.Content.ReadFromJsonAsync<QdrantSearchResponse>(ct);

            var result = new List<VecHit>();
            foreach (var item in json!.Result)
            {
                result.Add(new VecHit(item.Id, item.Score, item.Payload.Text, item.Payload.Source));
            }
            return result;
        }

        public async Task<List<VecHit>> GetLatestUserBehavior(
    int limit,
    string userId,
    string studyplanId,
    string dataType,
    CancellationToken ct = default)
        {
            var name = _activeCollection ?? _cfg.Qdrant.Collection;

            await EnsureBehaviorQueryIndexesAsync(name, ct);

            var body = new
            {
                limit = limit,
                with_payload = true,
                filter = new
                {
                    must = new object[]
                    {
                new
                {
                    key = "user_id",
                    match = new { value = userId }
                },
                new
                {
                    key = "studyplan_id",
                    match = new { value = studyplanId }
                },
                new
                {
                    key = "data_type",
                    match = new { value = dataType }
                }
                    }
                },
                order_by = new
                {
                    key = "created_at",
                    direction = "desc"
                }
            };

            var res = await _client.PostAsJsonAsync(
                $"collections/{name}/points/scroll",
                body,
                ct);

            await EnsureSuccessWithBodyAsync(res, ct);

            await using var stream = await res.Content.ReadAsStreamAsync(ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

            var result = new List<VecHit>();

            if (!doc.RootElement.TryGetProperty("result", out var resultElement))
            {
                return result;
            }

            JsonElement pointsElement;
            if (resultElement.ValueKind == JsonValueKind.Object
                && resultElement.TryGetProperty("points", out var nestedPoints)
                && nestedPoints.ValueKind == JsonValueKind.Array)
            {
                pointsElement = nestedPoints;
            }
            else if (resultElement.ValueKind == JsonValueKind.Array)
            {
                pointsElement = resultElement;
            }
            else
            {
                return result;
            }

            foreach (var item in pointsElement.EnumerateArray())
            {
                var id = item.TryGetProperty("id", out var idElement)
                    ? (idElement.ValueKind == JsonValueKind.String ? idElement.GetString() : idElement.GetRawText())
                    : string.Empty;

                string text = string.Empty;
                string? source = null;

                if (item.TryGetProperty("payload", out var payloadElement) && payloadElement.ValueKind == JsonValueKind.Object)
                {
                    if (payloadElement.TryGetProperty("text", out var textElement) && textElement.ValueKind == JsonValueKind.String)
                    {
                        text = textElement.GetString() ?? string.Empty;
                    }

                    if (payloadElement.TryGetProperty("source", out var sourceElement) && sourceElement.ValueKind == JsonValueKind.String)
                    {
                        source = sourceElement.GetString();
                    }
                }

                result.Add(new VecHit(id ?? string.Empty, 0, text, source));
            }

            return result;
        }


        public async Task UpsertAsync(IEnumerable<VectorPoint> points, CancellationToken ct = default)
        {
            var pointList = points.ToList();
            if (pointList.Count == 0) return;

            await EnsureCollectionAsync(pointList[0].Vector.Length, ct);

            var name = _activeCollection ?? _cfg.Qdrant.Collection;

            var payload = new
            {
                points = pointList.Select(v => new
                {
                    id = v.Id,
                    vector = v.Vector,
                    payload = new
                    {
                        text = v.Text,
                        source = v.Source,
                        user_id = v.UserId,
                        studyplan_id = v.StudyplanId,
                        data_type = v.DataType,
                        created_at = v.CreatedAt
                    }
                })
            };

            var res = await _client.PutAsJsonAsync($"collections/{name}/points?wait=true", payload, ct);
            await EnsureSuccessWithBodyAsync(res, ct);
        }
    }
}
