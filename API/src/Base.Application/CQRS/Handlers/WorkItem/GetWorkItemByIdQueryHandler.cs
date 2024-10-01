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
    public class GetWorkItemByIdQueryHandler(IWorkItemRepository repository, IMapper mapper) : IQueryHandler<GetWorkItemByIdQuery, ApiResponse<WorkItemDto>>
    {
        private readonly IWorkItemRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<WorkItemDto>> Handle(GetWorkItemByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var taskDb = await _repository.GetByIdAsync(request.IdTask);
                if (taskDb == null) throw new NotFoundException(nameof(WorkItem), request.IdTask);
                var infoResponse = _mapper.Map<WorkItemDto>(taskDb);
                return ApiResponse <WorkItemDto>.SuccessResponse(infoResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<WorkItemDto>.ErrorResponse(ex.Message);
            }
        }
    }
}
