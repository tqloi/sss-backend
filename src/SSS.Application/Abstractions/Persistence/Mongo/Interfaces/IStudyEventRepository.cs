using SSS.Domain.Entities.Tracking;

namespace SSS.Application.Abstractions.Persistence.Mongo.Interfaces
{
    public interface IStudyEventRepository
        : IMongoRepository<StudyEvent>
    {
        Task<IEnumerable<StudyEvent>> GetBySessionIdAsync(string sessionId);
        Task<IEnumerable<StudyEvent>> GetByUserIdAsync(string userId, string moduleId);
    }
}
