using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Persistence.Mongo.Documents
{
    [BsonCollection("ai_chat_messages")]
    public class AiChatMessageDocument : MongoDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ConversationId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        [BsonRepresentation(BsonType.String)]
        public AiMessageRole Role { get; set; }

        public string MessageContent { get; set; } = null!;

        [BsonIgnoreIfNull]
        public object? Context { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
