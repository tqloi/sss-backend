using MediatR;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.AI;

namespace SSS.Application.Features.AI.Chat.GetConversations
{
    public class GetConversationsHandler : IRequestHandler<GetConversationsQuery, IEnumerable<AiConversation>>
    {
        private readonly IAiConversationRepository _conversationRepo;

        public GetConversationsHandler(
            IAiConversationRepository conversationRepo)
        {
            _conversationRepo = conversationRepo;
        }

        public async Task<IEnumerable<AiConversation>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var conversations = await _conversationRepo.GetByUserIdAsync(userId);
            return conversations.OrderByDescending(c => c.LastMessageAt);
        }
    }
}
