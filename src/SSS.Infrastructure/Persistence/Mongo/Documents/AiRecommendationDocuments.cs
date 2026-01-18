using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Persistence.Mongo.Documents
{
    [BsonCollection("ai_recommendations")]
    public class AiRecommendationDocument : MongoDocument
    {
        public string UserId { get; set; } = null!;

        [BsonRepresentation(BsonType.String)]
        public AiRecommendationType Type { get; set; } = AiRecommendationType.Suggestion;

        public string ContentText { get; set; } = null!;

        [BsonRepresentation(BsonType.String)]
        public RelatedEntityType? RelatedType { get; set; }
        public string? RelatedId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
