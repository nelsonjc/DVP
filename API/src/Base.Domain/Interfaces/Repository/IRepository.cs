using System.Linq.Expressions;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Domain.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<PagedList<TEntity>> GetPaged(PaginationQueryFilter filter, Expression<Func<TEntity, bool>> where);
        Task SaveChangeAsync();
    }
}
