using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.Exceptions;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;

namespace TaskingSystem.Application.CQRS.Handlers
{
    public class DeleteWorkItemCommandHandler(IWorkItemRepository repository) : ICommandHandler<DeleteWorkItemCommand, ApiResponse<bool>>
    {
        private readonly IWorkItemRepository _repository = repository;

        public async Task<ApiResponse<bool>> Handle(DeleteWorkItemCommand command, CancellationToken cancellation)
        {
            try
            {
                var workItem = await _repository.GetByIdAsync(command.IdTask);
                if (workItem == null)
                    throw new NotFoundException(nameof(WorkItem), command.IdTask);

                await _repository.DeleteAsync(workItem.Id);
                return ApiResponse<bool>.SuccessResponse(true); ;
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(ex.Message);
            }
        }
    }
}
