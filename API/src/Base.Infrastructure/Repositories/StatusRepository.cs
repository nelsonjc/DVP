using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Infrastructure.Persistencia;

namespace TaskingSystem.Infrastructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public StatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(StatusSystem entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, Guid idUserUpdated)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<StatusSystem>> FindAsync(Expression<Func<StatusSystem, bool>> predicate)
        {
            return await _context.Set<StatusSystem>()
                .Include(u => u.UserCreated)
                .Include(r => r.UserUpdated)
             .Where(predicate)
             .ToListAsync();
        }

        public Task<IEnumerable<StatusSystem>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<StatusSystem> GetByIdAsync(Guid id)
        {
            return await _context.Set<StatusSystem>().FindAsync(id);
        }

        public Task<PagedList<StatusSystem>> GetPaged(PaginationQueryFilter filter, Expression<Func<StatusSystem, bool>> where)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangeAsync() => await _context.SaveChangesAsync();

        public Task UpdateAsync(StatusSystem entity)
        {
            throw new NotImplementedException();
        }
    }
}
