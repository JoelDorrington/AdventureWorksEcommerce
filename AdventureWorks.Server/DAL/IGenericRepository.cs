using AdventureWorks.Server.Entities;

namespace AdventureWorks.Server.DAL
{
    public interface IGenericRepository<T> where T : notnull, BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
