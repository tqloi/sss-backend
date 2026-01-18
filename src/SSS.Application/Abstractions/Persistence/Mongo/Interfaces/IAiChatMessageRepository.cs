using SSS.Domain.Entities.AI;

namespace SSS.Application.Abstractions.Persistence.Mongo.Interfaces
{
    public interface IAiChatMessageRepository
        : IMongoRepository<AiChatMessage>
    {
        Task<IEnumerable<AiChatMessage>> GetByConversationIdAsync(string conversationId);
    }
}
