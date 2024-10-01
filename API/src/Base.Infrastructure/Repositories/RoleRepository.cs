using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Infrastructure.Persistencia;

namespace TaskingSystem.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public Task AddAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Role>> FindAsync(Expression<Func<Role, bool>> predicate)
        {
            return await _context.Set<Role>()
           .Include(t => t.RolPermissions.OrderBy(x => x.Permission.Entity ))
               .ThenInclude(s => s.Permission)
               .ThenInclude(x => x.UserCreated)
           .Where(predicate)
           .OrderBy(r => r.RolPermissions.FirstOrDefault().Permission.Entity) 
           .ThenBy(r => r.RolPermissions.FirstOrDefault().Permission.ActionType) 
           .ToListAsync();

        }

        public Task<IEnumerable<Role>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Role>> GetPaged(PaginationQueryFilter filter, Expression<Func<Role, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangeAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}
