using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.AI.Chat.CreateConversation
{
    public class CreateConversationCommand : IRequest<CreateConversationResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public long RoadmapId { get; set; }
        public string? Title { get; set; }
    }

    public class CreateConversationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string ConversationId { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
