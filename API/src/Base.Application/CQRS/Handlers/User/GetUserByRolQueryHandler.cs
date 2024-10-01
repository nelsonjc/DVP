using AutoMapper;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.CQRS.Queries;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Exceptions;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;

namespace TaskingSystem.Application.CQRS.Handlers
{
    public class GetUserByRolQueryHandler(IUserRepository repository, IMapper mapper) : IQueryHandler<GetUserByRolQuery, ApiResponse<IEnumerable<UserDto>>>
    {
        private readonly IUserRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<IEnumerable<UserDto>>> Handle(GetUserByRolQuery query, CancellationToken cancellation)
        {
            try
            {
                var userDb = await _repository.GetByRol(query.RolName);
                if (userDb == null) throw new NotFoundException(nameof(User), Guid.NewGuid());
                var infoResponse = _mapper.Map<IEnumerable<UserDto>>(userDb);
                return ApiResponse<IEnumerable<UserDto>>.SuccessResponse(infoResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UserDto>>.ErrorResponse(ex.Message);
            }
        }
    }
}
