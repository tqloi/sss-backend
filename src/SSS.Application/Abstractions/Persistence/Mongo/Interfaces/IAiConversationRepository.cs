using SSS.Domain.Entities.AI;

namespace SSS.Application.Abstractions.Persistence.Mongo.Interfaces
{
    public interface IAiConversationRepository
            : IMongoRepository<AiConversation>
    {
        Task<IEnumerable<AiConversation>> GetByUserIdAsync(string userId);
        Task<AiConversation?> GetActiveByUserAsync(string userId);
        Task<AiConversation?> GetByUserAndRoadmapAsync(string userId, long roadmapId);
        Task<IEnumerable<AiConversation>> GetByUserAndRoadmapListAsync(string userId, long roadmapId);
    }
}

