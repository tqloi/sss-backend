namespace SSS.Application.Abstractions.Persistence.Mongo.Interfaces
{
    public interface IMongoRepository<T>
    {
        Task<T?> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
