using AutoMapper;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.CQRS.Queries;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Domain.Interfaces.Services;
using TaskingSystem.Domain.Options;

namespace TaskingSystem.Application.CQRS.Handlers
{
    public class GetAllWorkItemsQueryHandler(IOptions<PaginationOptions> paginationOptions, 
        ITreeModifierService treeModifierService,
        IMapper mapper, 
        IWorkItemRepository repository) : IQueryHandler<GetAllWorkItemsQuery, ApiResponse<PagedList<WorkItemDto>>>
    {
        private readonly PaginationOptions _paginationOptions = paginationOptions.Value;
        private readonly ITreeModifierService _treeModifierService = treeModifierService;
        private readonly IWorkItemRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<PagedList<WorkItemDto>>> Handle(GetAllWorkItemsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //Validation pagination
                if (request.PageNumber <= 0)
                    request.PageNumber = _paginationOptions.DefaultPageNumber;

                if (request.RowsOfPage <= 0)
                    request.RowsOfPage = _paginationOptions.DefaultPageSize;

                if (request.AllRows)
                    request.RowsOfPage = int.MaxValue;

                var where = GenerateFilters(request);
                PagedList<WorkItem> tasks = await _repository.GetPaged(request, where);

                var infoResult = _mapper.Map<List<WorkItemDto>>(tasks);
                var paginated = new PagedList<WorkItemDto>(infoResult, tasks.TotalCount, tasks.CurrentPage, tasks.PageSize);
                return ApiResponse<PagedList<WorkItemDto>>.SuccessResponse(paginated);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedList<WorkItemDto>>.ErrorResponse(ex.Message);
            }
        }

        private Expression<Func<WorkItem, bool>> GenerateFilters(GetAllWorkItemsQuery filters)
        {
            Expression<Func<WorkItem, bool>> where = x => x.Active;

            // Fecha de Creación
            if (filters.CreationDateStart.HasValue && filters.CreationDateEnd.HasValue)
                where = _treeModifierService.CombineAnd(where, x => x.DateCreated.Date >= filters.CreationDateStart.Value.Date && x.DateCreated.Date <= filters.CreationDateEnd.Value.Date);

            // Titulo
            if (!string.IsNullOrEmpty(filters.Title))
                where = _treeModifierService.CombineAnd(where, x => x.Title.ToLower().Contains(filters.Title.ToLower()));

            // Fecha de Vencimiento
            if (filters.DueDateStart.HasValue && filters.DueDateEnd.HasValue)
                where = _treeModifierService.CombineAnd(where, x => x.DueDate.Date >= filters.DueDateStart.Value.Date && x.DueDate.Date <= filters.DueDateEnd.Value.Date);

            // Usuario Asignado
            if (filters.IdUser.HasValue && filters.IdUser != Guid.Empty)
                where = _treeModifierService.CombineAnd(where, x => x.IdUser == filters.IdUser);

            // Estado
            if (filters.IdStatus.HasValue && filters.IdStatus != Guid.Empty)
                where = _treeModifierService.CombineAnd(where, x => x.IdStatus == filters.IdStatus);

            return where;
        }
    }
}
