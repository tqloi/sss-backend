using SSS.Domain.Entities.Tracking;

namespace SSS.Application.Abstractions.Persistence.Mongo.Interfaces
{
    public interface IPostHogEventRepository : IMongoRepository<PostHogEvent>
    {
    }
}
