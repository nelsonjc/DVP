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
    public class GetStatusAllQueryHandler(IMapper mapper, IStatusRepository repository) : IQueryHandler<GetStatusAllQuery, ApiResponse<IEnumerable<StatusDto>>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IStatusRepository _repository = repository;

        public async Task<ApiResponse<IEnumerable<StatusDto>>> Handle(GetStatusAllQuery query, CancellationToken cancellation)
        {
            try
            {
                var statusDb = query.All ? await _repository.FindAsync(x => x.Entity.ToUpper() == query.Entity) : await _repository.FindAsync(x => x.Active == query.Active && x.Entity.ToUpper() == query.Entity);
                if (statusDb == null) throw new NotFoundException(nameof(StatusSystem), Guid.NewGuid());
                var infoResponse = _mapper.Map<IEnumerable<StatusDto>>(statusDb);
                return ApiResponse<IEnumerable<StatusDto>>.SuccessResponse(infoResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<StatusDto>>.ErrorResponse(ex.Message);
            }
        }
    }
}
