using MediatR;
using SSS.Domain.Entities.AI;

namespace SSS.Application.Features.AI.Chat.GetHistory
{
    public class GetHistoryQuery : IRequest<IEnumerable<AiChatMessage>>
    {
        public string ConversationId { get; set; } = null!;
    }
}
