using SSS.Application.Abstractions.External.AiServices;
using SSS.Application.Abstractions.External.AiServices.Vector;
using SSS.Domain.Entities.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Infrastructure.External.AiServices.Vector
{
    public class QdrantClient : IQdrantClient
    {
        private readonly HttpClient _client;
        private readonly AiConfig _cfg;
        private string? _activeCollection;
        public QdrantClient(IHttpClientFactory client, AiConfig cfg)
        {

            _cfg = cfg;
            _client = client.CreateClient(nameof(QdrantClient));
            _client.BaseAddress = new Uri(_cfg.Qdrant.Endpoint.Trim('/') + "/");
        }
        public async Task EnsureCollectionAsync(int vectorSize, CancellationToken ct = default)
        {
            var baseName = _cfg.Qdrant.Collection;
            var name = $"{baseName}_{vectorSize}";
            _activeCollection = name;
            // check exists
            var check = await _client.GetAsync($"collections/{name}", ct);
            if (check.IsSuccessStatusCode) return;
            var body = new
            {
                vectors = new { size = vectorSize, distance = "Cosine" }

            };

            var resp = await _client.PutAsJsonAsync($"collections/{name}", body, ct);
            resp.EnsureSuccessStatusCode();
        }

        public async Task<List<VecHit>> SearchAsync(float[] query, int topK, CancellationToken ct = default)
        {
            var name = _activeCollection ?? _cfg.Qdrant.Collection;
            var body = new
            {
                vector = query,
                top = topK,
                with_payload = true
            };

            var res = await _client.PostAsJsonAsync($"collections/{name}/points/search", body, ct);
            res.EnsureSuccessStatusCode();
            var json = await res.Content.ReadFromJsonAsync<QdrantSearchResponse>(cancellationToken: ct);

            var result = new List<VecHit>();
            foreach (var item in json!.Result)
            {
                result.Add(new VecHit(item.Id, item.Score, item.Payload.Text, item.Payload.Source));
            }
            return result;
        }

        public async Task<List<VecHit>> SearchByUserId(float[] vector, int topK, string userId, string dataType, CancellationToken ct = default)
        {

            var name = _activeCollection ?? _cfg.Qdrant.Collection;

            var body = new
            {
                vector = vector,
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

            res.EnsureSuccessStatusCode();

            var json = await res.Content.ReadFromJsonAsync<QdrantSearchResponse>(ct);

            var result = new List<VecHit>();
            foreach (var item in json!.Result)
            {
                result.Add(new VecHit(item.Id, item.Score, item.Payload.Text, item.Payload.Source));
            }
            return result;
        }


        public async Task UpsertAsync(IEnumerable<VectorPoint> points, CancellationToken ct = default)
        {
            var name = _activeCollection ?? _cfg.Qdrant.Collection;

            var payload = new
            {
                points = points.Select(v => new
                {
                    id = v.Id,
                    vector = v.Vector,
                    payload = new
                    {
                        text = v.Text,
                        source = v.Source,
                        user_id = v.UserId,
                        data_type = v.DataType,
                        created_at = v.CreatedAt
                    }
                })
            };

            var res = await _client.PutAsJsonAsync($"collections/{name}/points?wait=true", payload, ct);
            res.EnsureSuccessStatusCode();
        }
    }
}
