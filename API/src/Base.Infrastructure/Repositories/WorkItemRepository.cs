using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Infrastructure.Persistencia;

namespace TaskingSystem.Infrastructure.Repositories
{
    public class WorkItemRepository : IWorkItemRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        

        public async Task AddAsync(WorkItem entity)
        {
            await _context.WorkItems.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, Guid idUserUpdated)
        {
            var WorkItemDb = await GetByIdAsync(id);
            if (WorkItemDb != null)
            {
                WorkItemDb.Active = false;
                WorkItemDb.IdUserUpdated = idUserUpdated;
                WorkItemDb.DateUpdated = DateTime.UtcNow.AddHours(-5);
                _context.WorkItems.Update(WorkItemDb);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<WorkItem>> FindAsync(Expression<Func<WorkItem, bool>> predicate)
        {
            return await _context.Set<WorkItem>()
                .Include(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.UserCreated)
                .Include(x => x.UserUpdated)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkItem>> GetAllAsync()
        {
            IQueryable<WorkItem> query = _context.Set<WorkItem>().AsQueryable();
            query = query.Include(x => x.Status);
            return await query.ToListAsync();
        }

        public async Task<WorkItem?> GetByIdAsync(Guid id)
        {
            var result = await _context.WorkItems
                .Include(x => x.Logs)
                .Include(x => x.Flows)
                    .ThenInclude(x => x.PreviousStatus)
                .Include(x => x.Flows)
                    .ThenInclude(x => x.NewStatus)
                .Include(x => x.Flows)
                    .ThenInclude(x => x.UserCreated)
                .Include(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.UserCreated)
                .Include(x => x.UserUpdated)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result != null && result.Active)
            {
                // Ordenar Logs y Flows en memoria
                result.Logs = result.Logs.OrderByDescending(log => log.DateCreated).ToList(); 
                result.Flows = result.Flows.OrderByDescending(flow => flow.DateCreated).ToList(); 
                return result;
            }

            return null;
        }

        public async Task<WorkItem> GetByIdSimple(Guid id)
        {
            return await _context.WorkItems
                .Include(w => w.Logs)
                .Include(w => w.Flows)
                .FirstOrDefaultAsync(w => w.Id == id);

        }

        public async Task<PagedList<WorkItem>> GetPaged(PaginationQueryFilter filter, Expression<Func<WorkItem, bool>> where)
        {
            IQueryable<WorkItem> query = _context.Set<WorkItem>().Where(where).AsQueryable();
            int totalItems;

            query = query.Include(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.UserCreated)
                .Include(x => x.UserUpdated)
                .AsQueryable();

            totalItems = await query.CountAsync();
            query = filter.OrderAsc.HasValue && filter.OrderAsc.Value ? query.OrderBy(x => x.DateCreated) : query.OrderByDescending(x => x.DateCreated);
            query = query.Skip((filter.PageNumber - 1) * filter.RowsOfPage).Take(filter.RowsOfPage);

            try
            {
                var items = await query.ToListAsync();
                return new PagedList<WorkItem>(items, totalItems, filter.PageNumber, filter.RowsOfPage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al ejecutar la consulta: {ex.Message}", ex);
            }
        }

        public async Task SaveChangeAsync() => await _context.SaveChangesAsync();

        public async Task UpdateAsync(WorkItem entity)
        {
            _context.WorkItems.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
