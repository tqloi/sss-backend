using SSS.Domain.Enums;

namespace SSS.Domain.Entities.AI
{
    public class AiChatMessage
    {
        public string Id { get; set; } = null!;
        public string ConversationId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public AiMessageRole Role { get; set; }
        public string MessageContent { get; set; } = null!;
        public object? Context { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
