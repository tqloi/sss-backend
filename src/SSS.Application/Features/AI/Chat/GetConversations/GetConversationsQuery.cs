using MediatR;
using SSS.Domain.Entities.AI;

namespace SSS.Application.Features.AI.Chat.GetConversations
{
    public class GetConversationsQuery : IRequest<IEnumerable<AiConversation>>
    {
        public string UserId { get; set; } = null!;
        public long? RoadmapId { get; set; }
    }
}
