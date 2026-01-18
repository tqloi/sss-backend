using AutoMapper;
using MongoDB.Driver;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.Tracking;
using SSS.Infrastructure.Persistence.Mongo.Documents;

namespace SSS.Infrastructure.Persistence.Mongo.Repositories
{
    public class StudyEventRepository
         : MongoRepository<StudyEvent, StudyEventDocument>,
           IStudyEventRepository
    {
        public StudyEventRepository(
            MongoContext context,
            IMapper mapper)
            : base(context, mapper, "study_events")
        {
        }

        public async Task<IEnumerable<StudyEvent>> GetBySessionIdAsync(string sessionId)
        {
            var docs = await _collection
                .Find(x => x.SessionId == sessionId)
                .SortBy(x => x.EventTimestamp)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StudyEvent>>(docs);
        }
    }
}
