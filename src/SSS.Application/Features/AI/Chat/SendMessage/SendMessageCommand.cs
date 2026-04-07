using MediatR;

namespace SSS.Application.Features.AI.Chat.SendMessage
{
    public class SendMessageCommand : IRequest<SendMessageResult>
    {
        public string UserId { get; set; } = null!;
        public string? ConversationId { get; set; }
        public long RoadmapId { get; set; }
        public string MessageContent { get; set; } = null!;
        public List<long>? ModuleIds { get; set; }
        public List<long>? TaskIds { get; set; }
    }

    public class SendMessageResult
    {
        public string ConversationId { get; set; } = null!;
        public string MessageId { get; set; } = null!;
        public string AiResponse { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}

