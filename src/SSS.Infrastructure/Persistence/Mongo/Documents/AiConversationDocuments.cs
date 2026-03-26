using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SSS.Infrastructure.Persistence.Mongo.Documents
{
    [BsonCollection("ai_conversations")]
    [BsonIgnoreExtraElements]
    public class AiConversationDocument : MongoDocument
    {
        public string UserId { get; set; } = null!;
        public long RoadmapId { get; set; }
        public string Title { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime LastMessageAt { get; set; }

        public bool IsActive { get; set; }
    }
}

