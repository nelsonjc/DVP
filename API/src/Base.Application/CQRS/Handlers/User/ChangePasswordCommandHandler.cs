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
    public class ChangePasswordCommandHandler(
        IMapper mapper,
        IUserRepository repository,
        IPasswordService passwordService) : ICommandHandler<ChangePasswordCommand, ApiResponse<bool>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _repository = repository;
        private readonly IPasswordService _passwordService = passwordService;

        public async Task<ApiResponse<bool>> Handle(ChangePasswordCommand command, CancellationToken cancellation)
        {
            try
            {
                var user = await _repository.GetByIdAsync(command.IdUser) ?? throw new NotFoundException(nameof(User), command.IdUser);
                var hashedPassword = _passwordService.GenerateHash(command.Password);
                await _repository.ChangePassword(command.IdUser, hashedPassword, command.IdUserUpdated);
                return ApiResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                
                return ApiResponse<bool>.ErrorResponse(ex.Message);
            }
        }
    }
}
