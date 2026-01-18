using SSS.Domain.Enums;

namespace SSS.Domain.Entities.AI
{
    public class AiRecommendation
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public AiRecommendationType Type { get; set; } = AiRecommendationType.Suggestion;
        public string ContentText { get; set; } = null!;
        public RelatedEntityType? RelatedType { get; set; }
        public string? RelatedId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
