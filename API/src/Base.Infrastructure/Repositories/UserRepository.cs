using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Infrastructure.Persistencia;

namespace TaskingSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var userDb = await GetByIdAsync(id);
            if (userDb != null)
            {
                userDb.Active = false;
                _context.Users.Update(userDb);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Set<User>()
                .Include(t => t.Tasks)
                    .ThenInclude(s => s.Status)
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolPermissions) 
                        .ThenInclude(rp => rp.Permission)
                .Include(u => u.UserCreated)
                .Include(u => u.UserUpdated)
                .Where(predicate) 
                .ToListAsync(); 
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(Guid id) => await _context.Users.FindAsync(id);

        public async Task<IEnumerable<User>> GetByRol(string rolName)
        {
            return await _context.Set<User>()
               .Include(u => u.Role)
               .Where(x => x.Active && x.Role.Name.ToLower() == rolName.ToLower())
               .ToListAsync();
        }

        public Task<PagedList<User>> GetPaged(PaginationQueryFilter filter, Expression<Func<User, bool>> where)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangeAsync() => await _context.SaveChangesAsync();

        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
