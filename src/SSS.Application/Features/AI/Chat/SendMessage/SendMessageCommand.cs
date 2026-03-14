using MediatR;
using SSS.Domain.Enums;

namespace SSS.Application.Features.AI.Chat.SendMessage
{
    public class SendMessageCommand : IRequest<SendMessageResult>
    {
        public string UserId { get; set; } = null!;
        public string? ConversationId { get; set; }
        public string MessageContent { get; set; } = null!;
        public RelatedEntityType? RelatedType { get; set; }
        public string? RelatedId { get; set; }
    }

    public class SendMessageResult
    {
        public string ConversationId { get; set; } = null!;
        public string MessageId { get; set; } = null!;
        public string AiResponse { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
