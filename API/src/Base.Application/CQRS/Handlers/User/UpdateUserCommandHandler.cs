using AutoMapper;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.Exceptions;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Domain.Interfaces.Services;

namespace TaskingSystem.Application.CQRS.Handlers
{
    public class UpdateUserCommandHandler(
        IMapper mapper,
        IUserRepository repository,
        IPasswordService passwordService) : ICommandHandler<UpdateUserCommand, ApiResponse<bool>>
    {

        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _repository = repository;
        private readonly IPasswordService _passwordService = passwordService;

        public async Task<ApiResponse<bool>> Handle(UpdateUserCommand command, CancellationToken cancellation)
        {
            try
            {
                var user = await _repository.GetByIdAsync(command.Id) ?? throw new NotFoundException(nameof(User), command.Id);
                user.FullName = command.FullName;
                user.UserName = command.UserName;
                user.IdUserUpdated = command.IdUserUpdated;
                user.Active = command.Active;
                user.IdRole = command.IdRole;
                user.DateUpdated = DateTime.UtcNow.AddHours(-5);
                
                await _repository.UpdateAsync(user);
                return ApiResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(ex.Message);
            }
        }
    }
}
