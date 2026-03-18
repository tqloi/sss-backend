using AutoMapper;
using MongoDB.Driver;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.AI;
using SSS.Infrastructure.Persistence.Mongo.Documents;

namespace SSS.Infrastructure.Persistence.Mongo.Repositories
{
    public class AiConversationRepository
      : MongoRepository<AiConversation, AiConversationDocument>,
        IAiConversationRepository
    {
        public AiConversationRepository(
            MongoContext context,
            IMapper mapper)
            : base(context, mapper, "ai_conversations")
        {
        }

        public async Task<IEnumerable<AiConversation>> GetByUserIdAsync(string userId)
        {
            var docs = await _collection
                .Find(x => x.UserId == userId)
                .SortByDescending(x => x.LastMessageAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AiConversation>>(docs);
        }

        public async Task<AiConversation?> GetActiveByUserAsync(string userId)
        {
            var doc = await _collection
                .Find(x => x.UserId == userId && x.IsActive)
                .FirstOrDefaultAsync();

            return doc == null ? null : _mapper.Map<AiConversation>(doc);
        }

        public async Task<AiConversation?> GetByUserAndRoadmapAsync(string userId, long roadmapId)
        {
            var doc = await _collection
                .Find(x => x.UserId == userId && x.RoadmapId == roadmapId && x.IsActive)
                .SortByDescending(x => x.LastMessageAt)
                .FirstOrDefaultAsync();

            return doc == null ? null : _mapper.Map<AiConversation>(doc);
        }

        public async Task<IEnumerable<AiConversation>> GetByUserAndRoadmapListAsync(string userId, long roadmapId)
        {
            var docs = await _collection
                .Find(x => x.UserId == userId && x.RoadmapId == roadmapId)
                .SortByDescending(x => x.LastMessageAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AiConversation>>(docs);
        }
    }
}

