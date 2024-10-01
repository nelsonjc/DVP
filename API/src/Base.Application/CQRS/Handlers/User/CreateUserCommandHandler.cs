using AutoMapper;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Domain.Interfaces.Services;

namespace TaskingSystem.Application.CQRS.Handlers
{
    public class CreateUserCommandHandler(
        IMapper mapper, 
        IUserRepository repository,
        IPasswordService passwordService) : ICommandHandler<CreateUserCommand, ApiResponse<Guid>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _repository = repository;
        private readonly IPasswordService _passwordService = passwordService;

        public async Task<ApiResponse<Guid>> Handle(CreateUserCommand command, CancellationToken cancellation)
        {
            try
            {
                var userDb = _mapper.Map<User>(command);
                userDb.Active = true;
                userDb.Password = _passwordService.GenerateHash(command.Password);

                await _repository.AddAsync(userDb);
                return ApiResponse<Guid>.SuccessResponse(userDb.Id);
            }
            catch (Exception ex)
            {
                return ApiResponse<Guid>.ErrorResponse(ex.Message);
            }
        }
    }
}
