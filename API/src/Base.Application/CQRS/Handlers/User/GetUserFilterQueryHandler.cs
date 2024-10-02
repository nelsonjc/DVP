using AutoMapper;
using System.Linq.Expressions;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.CQRS.Queries;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Exceptions;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Domain.Interfaces.Services;

namespace TaskingSystem.Application.CQRS.Handlers
{
    public class GetUserFilterQueryHandler(
        ITreeModifierService treeModifierService,
        IUserRepository repository, 
        IMapper mapper) : IQueryHandler<GetUserFilterQuery, ApiResponse<UserDto[]>>
    {
        private readonly ITreeModifierService _treeModifierService = treeModifierService;
        private readonly IUserRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<UserDto[]>> Handle(GetUserFilterQuery query, CancellationToken cancellation)
        {
            try
            {
                var filters = GenerateFilters(query);
                var userDb = await _repository.FindAsync(filters);
                if (userDb == null) throw new NotFoundException(nameof(User), Guid.NewGuid());
                var infoResponse = _mapper.Map<UserDto[]>(userDb);
                return ApiResponse<UserDto[]>.SuccessResponse(infoResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto[]>.ErrorResponse(ex.Message);
            }
        }

        private Expression<Func<User, bool>> GenerateFilters(GetUserFilterQuery filters)
        {
            Expression<Func<User, bool>> where = filters.AllRows.HasValue && filters.AllRows.Value ? 
                x => x.Active == x.Active : 
                x => x.Active;

            // Nombre rol
            if (!string.IsNullOrEmpty(filters.FullName))
                where = _treeModifierService.CombineAnd(where, x => x.FullName.ToLower().Contains(filters.FullName.ToLower()));


            return where;
        }
    }
}
