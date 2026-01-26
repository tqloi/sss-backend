using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Abstractions.External.AI.Vector
{
    public interface IQdrantClient
    {
        Task EnsureCollectionAsync(int vectorSize, CancellationToken ct = default);
        Task UpsertAsync(IEnumerable<VectorPoint> vectors, CancellationToken ct = default);
        Task<List<VecHit>> SearchAsync(float[] query, int topK, CancellationToken ct = default);
        Task<List<VecHit>> SearchByUserId(float[] vector, int topK, string userId, string dataType, CancellationToken ct = default);
    }

    public record VectorPoint(string Id, float[] Vector, string Text, string? Source, string UserId, string DataType, DateTime CreatedAt);
    public record VecHit(string Id, float Score, string Text, string? Source);

    public class QdrantSearchResponse
    {
        public List<QdrantHit> Result { get; set; }
    }

    public class QdrantHit
    {
        public string Id { get; set; }
        public float Score { get; set; }
        public QdrantPayload Payload { get; set; }
    }

    public class QdrantPayload
    {
        public string Text { get; set; }
        public string Source { get; set; }
    }
}
