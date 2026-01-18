using AutoMapper;
using MongoDB.Driver;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.AI;
using SSS.Domain.Enums;
using SSS.Infrastructure.Persistence.Mongo.Documents;

namespace SSS.Infrastructure.Persistence.Mongo.Repositories
{
    public class AiContextLogRepository
           : MongoRepository<AiContextLog, AiContextLogDocument>,
             IAiContextLogRepository
    {
        public AiContextLogRepository(
            MongoContext context,
            IMapper mapper)
            : base(context, mapper, "ai_context_logs")
        {
        }

        public async Task<IEnumerable<AiContextLog>> GetByUserIdAsync(string userId)
        {
            var docs = await _collection
                .Find(x => x.UserId == userId)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AiContextLog>>(docs);
        }

        public async Task<IEnumerable<AiContextLog>> GetByRelatedEntityAsync(
            RelatedEntityType type, string relatedId)
        {
            var docs = await _collection
                .Find(x => x.RelatedType == type && x.RelatedId == relatedId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AiContextLog>>(docs);
        }
    }
}
