namespace SSS.Domain.Entities.AI
{
    public class AiConversation
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public long RoadmapId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime LastMessageAt { get; set; }
        public bool IsActive { get; set; }
    }
}
