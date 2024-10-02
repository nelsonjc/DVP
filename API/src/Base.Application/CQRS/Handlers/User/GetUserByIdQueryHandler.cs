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
    public class GetUserByIdQueryHandler(IUserRepository repository, IMapper mapper) : IQueryHandler<GetUserByIdQuery, ApiResponse<UserDto>>
    {
        private readonly IUserRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellation)
        {
            try
            {
                var userDb = (await _repository.FindAsync(x => x.Id == query.IdUser)).FirstOrDefault();
                if (userDb == null) throw new NotFoundException(nameof(User), query.IdUser);
                var infoResponse = _mapper.Map<UserDto>(userDb);
                return ApiResponse<UserDto>.SuccessResponse(infoResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResponse(ex.Message);
            }
        }
    }
}
