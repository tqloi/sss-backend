using SSS.Domain.Enums;

namespace SSS.Domain.Entities.AI
{
    public class AiConversation
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public RelatedEntityType? RelatedType { get; set; }
        public string? RelatedId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastMessageAt { get; set; }
        public bool IsActive { get; set; }
    }
}
