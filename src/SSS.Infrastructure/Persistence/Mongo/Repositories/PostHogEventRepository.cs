using AutoMapper;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.Tracking;
using SSS.Infrastructure.Persistence.Mongo.Documents;

namespace SSS.Infrastructure.Persistence.Mongo.Repositories
{
    public class PostHogEventRepository : MongoRepository<PostHogEvent, PostHogEventDocument>, IPostHogEventRepository
    {
        public PostHogEventRepository(MongoContext context, IMapper mapper) 
            : base(context, mapper, "posthog_events")
        {
        }
    }
}
