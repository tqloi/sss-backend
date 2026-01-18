using SSS.Domain.Entities.AI;
using SSS.Domain.Enums;

namespace SSS.Application.Abstractions.Persistence.Mongo.Interfaces
{
    public interface IAiContextLogRepository
        : IMongoRepository<AiContextLog>
    {
        Task<IEnumerable<AiContextLog>> GetByUserIdAsync(string userId);
        Task<IEnumerable<AiContextLog>> GetByRelatedEntityAsync(
            RelatedEntityType type, string relatedId);
    }
}
