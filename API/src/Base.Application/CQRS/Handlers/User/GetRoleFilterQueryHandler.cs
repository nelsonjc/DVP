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
    public class GetRoleFilterQueryHandler(
        ITreeModifierService treeModifierService,
        IRoleRepository repository, 
        IMapper mapper) : IQueryHandler<GetRoleFilterQuery, ApiResponse<IEnumerable<RoleDto>>>
    {
        private readonly ITreeModifierService _treeModifierService = treeModifierService;
        private readonly IRoleRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<IEnumerable<RoleDto>>> Handle(GetRoleFilterQuery query, CancellationToken cancellation)
        {
            try
            {
                var filters = GenerateFilters(query);
                var roleDB = await _repository.FindAsync(filters);
                if (roleDB == null) throw new NotFoundException(nameof(Role), Guid.NewGuid());
                var infoResponse = _mapper.Map<IEnumerable<RoleDto>>(roleDB);
                return ApiResponse<IEnumerable<RoleDto>>.SuccessResponse(infoResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<RoleDto>>.ErrorResponse(ex.Message);
            }
        }

        private Expression<Func<Role, bool>> GenerateFilters(GetRoleFilterQuery filters)
        {
            Expression<Func<Role, bool>> where = x => x.Active;

            // Nombre rol
            if (!string.IsNullOrEmpty(filters.RoleName))
                where = _treeModifierService.CombineAnd(where, x => x.Name.ToLower().Contains(filters.RoleName.ToLower()));

            return where;
        }
    }
}
