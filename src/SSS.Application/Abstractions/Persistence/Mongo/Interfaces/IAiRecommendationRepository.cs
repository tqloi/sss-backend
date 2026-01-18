using SSS.Domain.Entities.AI;

namespace SSS.Application.Abstractions.Persistence.Mongo.Interfaces
{
    public interface IAiRecommendationRepository
       : IMongoRepository<AiRecommendation>
    {
        Task<IEnumerable<AiRecommendation>> GetUnreadByUserIdAsync(string userId);
        Task MarkAsReadAsync(string recommendationId);
    }
}
