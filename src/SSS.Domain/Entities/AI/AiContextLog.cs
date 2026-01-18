using SSS.Domain.Enums;

namespace SSS.Domain.Entities.AI
{
    public class AiContextLog
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? ConversationId { get; set; }
        public string PromptTemplateUsed { get; set; } = null!;
        public object InputContextSnapshot { get; set; } = null!;
        public object AiResponseRaw { get; set; } = null!;
        public RelatedEntityType? RelatedType { get; set; }
        public string? RelatedId { get; set; }

        /// <summary>
        /// 1 = positive, -1 = negative, null = not rated
        /// </summary>
        public int? Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
