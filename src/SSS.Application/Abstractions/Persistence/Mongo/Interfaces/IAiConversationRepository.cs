using SSS.Domain.Entities.AI;
using SSS.Domain.Enums;

namespace SSS.Application.Abstractions.Persistence.Mongo.Interfaces
{
    public interface IAiConversationRepository
            : IMongoRepository<AiConversation>
    {
        Task<IEnumerable<AiConversation>> GetByUserIdAsync(string userId);
        Task<AiConversation?> GetActiveByUserAsync(string userId);
        Task<IEnumerable<AiConversation>> GetByRelatedEntityAsync(
            RelatedEntityType type, string relatedId);
    }
}
