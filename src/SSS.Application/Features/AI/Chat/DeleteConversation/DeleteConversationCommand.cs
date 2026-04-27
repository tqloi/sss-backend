using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.AI.Chat.DeleteConversation
{
    public class DeleteConversationCommand : IRequest<DeleteConversationResult>
    {
        public string ConversationId { get; set; } = null!;

        [JsonIgnore]
        public string UserId { get; set; } = null!;
    }

    public class DeleteConversationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }
}