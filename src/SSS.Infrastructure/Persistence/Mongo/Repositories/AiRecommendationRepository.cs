using AutoMapper;
using MongoDB.Driver;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.AI;
using SSS.Infrastructure.Persistence.Mongo.Documents;

namespace SSS.Infrastructure.Persistence.Mongo.Repositories
{
    public class AiRecommendationRepository
          : MongoRepository<AiRecommendation, AiRecommendationDocument>,
            IAiRecommendationRepository
    {
        public AiRecommendationRepository(
            MongoContext context,
            IMapper mapper)
            : base(context, mapper, "ai_recommendations")
        {
        }

        public async Task<IEnumerable<AiRecommendation>> GetUnreadByUserIdAsync(string userId)
        {
            var docs = await _collection
                .Find(x => x.UserId == userId && !x.IsRead)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AiRecommendation>>(docs);
        }

        public async Task MarkAsReadAsync(string recommendationId)
        {
            var update = Builders<AiRecommendationDocument>.Update
                .Set(x => x.IsRead, true);

            await _collection.UpdateOneAsync(
                x => x.Id == recommendationId,
                update);
        }
    }
}
