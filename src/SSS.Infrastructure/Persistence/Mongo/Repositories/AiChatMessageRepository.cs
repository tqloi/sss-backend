using AutoMapper;
using MongoDB.Driver;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.AI;
using SSS.Infrastructure.Persistence.Mongo.Documents;

namespace SSS.Infrastructure.Persistence.Mongo.Repositories
{
    public class AiChatMessageRepository
            : MongoRepository<AiChatMessage, AiChatMessageDocument>,
              IAiChatMessageRepository
    {
        public AiChatMessageRepository(
            MongoContext context,
            IMapper mapper)
            : base(context, mapper, "ai_chat_messages")
        {
        }

        public async Task<IEnumerable<AiChatMessage>> GetByConversationIdAsync(string conversationId)
        {
            var docs = await _collection
                .Find(x => x.ConversationId == conversationId)
                .SortBy(x => x.Timestamp)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AiChatMessage>>(docs);
        }
    }
}
