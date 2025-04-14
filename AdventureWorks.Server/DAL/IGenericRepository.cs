using AdventureWorks.Server.DAL.QueryParameters;
using AdventureWorks.Server.Entities;

namespace AdventureWorks.Server.DAL
{
    public interface IGenericRepository<T> where T : notnull, BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync(GetParameters args);
        Task<T> GetByIdAsync(int id, IJoinParameter[]? j=null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
