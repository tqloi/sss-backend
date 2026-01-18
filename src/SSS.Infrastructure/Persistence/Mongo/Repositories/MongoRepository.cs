using AutoMapper;
using MongoDB.Driver;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Infrastructure.Persistence.Mongo.Documents;

namespace SSS.Infrastructure.Persistence.Mongo.Repositories
{
    public abstract class MongoRepository<TDomain, TDocument>
        : IMongoRepository<TDomain>
        where TDocument : MongoDocument
    {
        protected readonly IMongoCollection<TDocument> _collection;
        protected readonly IMapper _mapper;

        protected MongoRepository(
            MongoContext context,
            IMapper mapper,
            string collectionName)
        {
            _collection = context.GetCollection<TDocument>(collectionName);
            _mapper = mapper;
        }

        public async Task<TDomain?> GetByIdAsync(string id)
        {
            var doc = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return doc == null ? default : _mapper.Map<TDomain>(doc);
        }

        public async Task<IEnumerable<TDomain>> GetAllAsync()
        {
            var docs = await _collection.Find(_ => true).ToListAsync();
            return _mapper.Map<IEnumerable<TDomain>>(docs);
        }

        public async Task AddAsync(TDomain entity)
        {
            var doc = _mapper.Map<TDocument>(entity);
            await _collection.InsertOneAsync(doc);
            _mapper.Map(doc, entity); // sync Id back
        }

        public async Task UpdateAsync(TDomain entity)
        {
            var doc = _mapper.Map<TDocument>(entity);
            await _collection.ReplaceOneAsync(x => x.Id == doc.Id, doc);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
