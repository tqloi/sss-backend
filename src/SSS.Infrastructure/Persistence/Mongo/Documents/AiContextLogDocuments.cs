using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Persistence.Mongo.Documents
{
    [BsonCollection("ai_context_logs")]
    public class AiContextLogDocument : MongoDocument
    {
        public string UserId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public string? ConversationId { get; set; }

        public string PromptTemplateUsed { get; set; } = null!;

        public object InputContextSnapshot { get; set; } = null!;

        public object AiResponseRaw { get; set; } = null!;

        [BsonRepresentation(BsonType.String)]
        public RelatedEntityType? RelatedType { get; set; }
        public string? RelatedId { get; set; }

        /// <summary>
        /// 1 = positive, -1 = negative, null = not rated
        /// </summary>
        public int? Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
