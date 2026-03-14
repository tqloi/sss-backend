using MediatR;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.AI;

namespace SSS.Application.Features.AI.Chat.GetHistory
{
    public class GetHistoryHandler : IRequestHandler<GetHistoryQuery, IEnumerable<AiChatMessage>>
    {
        private readonly IAiChatMessageRepository _chatMessageRepo;

        public GetHistoryHandler(IAiChatMessageRepository chatMessageRepo)
        {
            _chatMessageRepo = chatMessageRepo;
        }

        public async Task<IEnumerable<AiChatMessage>> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
        {
            var messages = await _chatMessageRepo.GetByConversationIdAsync(request.ConversationId);
            return messages.OrderBy(m => m.Timestamp);
        }
    }
}
