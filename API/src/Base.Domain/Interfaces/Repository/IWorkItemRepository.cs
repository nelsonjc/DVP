using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Domain.Interfaces.Repository
{
    public interface IWorkItemRepository : IRepository<WorkItem>
    {
        Task<WorkItem> GetByIdSimple(Guid id);

    }
}
