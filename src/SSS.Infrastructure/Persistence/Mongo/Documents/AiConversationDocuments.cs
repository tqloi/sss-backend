using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Persistence.Mongo.Documents
{
    [BsonCollection("ai_conversations")]
    public class AiConversationDocument : MongoDocument
    {
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;

        [BsonRepresentation(BsonType.String)]
        public RelatedEntityType? RelatedType { get; set; }
        public string? RelatedId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastMessageAt { get; set; }

        public bool IsActive { get; set; }
    }
}
