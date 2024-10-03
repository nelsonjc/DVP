using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.Exceptions;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;

namespace TaskingSystem.Application.CQRS.Handlers
{
    public class ActivateUserCommandHandler(IUserRepository repository) : ICommandHandler<ActivateUserCommand, ApiResponse<bool>>
    {

        private readonly IUserRepository _repository = repository;

        public async Task<ApiResponse<bool>> Handle(ActivateUserCommand command, CancellationToken cancellation)
        {
            try
            {
                var userDb = await _repository.GetByIdAsync(command.IdUser) ?? throw new NotFoundException(nameof(User), command.IdUser);
                await _repository.ActivateAsync(userDb.Id, command.IdUserUpdated);
                return ApiResponse<bool>.SuccessResponse(true); ;
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(ex.Message);
            }
        }

    }
    
    
}
