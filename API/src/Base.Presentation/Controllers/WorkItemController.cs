using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.CQRS.Queries;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkItemController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher = queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateTask([FromBody] CreateWorkItemCommand command, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<CreateWorkItemCommand, ApiResponse<Guid>>(command, cancellationToken);
            return result.Success ?
                CreatedAtAction(nameof(GetTaskById), new { id = result.Data }, result) :
                BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateWorkItemCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            var result = await _commandDispatcher.Dispatch<UpdateWorkItemCommand, ApiResponse<bool>>(command, cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("change-status/{id}")]
        public async Task<IActionResult> ChageStatus(Guid id, [FromBody] ChangeStatusWorkItemCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            var result = await _commandDispatcher.Dispatch<ChangeStatusWorkItemCommand, ApiResponse<bool>>(command, cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllWorkItemsQuery query, CancellationToken cancellationToken)
        {
            
            var dataResult = await _queryDispatcher.Dispatch<GetAllWorkItemsQuery, ApiResponse<PagedList<WorkItemDto>>>(query, cancellationToken);

            if (dataResult.Success)
            {
                var metadata = new
                {
                    dataResult.Data.TotalCount,
                    dataResult.Data.PageSize,
                    dataResult.Data.CurrentPage,
                    dataResult.Data.TotalPages,
                    dataResult.Data.HasPreviousPage,
                    dataResult.Data.HasNextPage,
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(new { data = dataResult, metadata });
            }

            return BadRequest(dataResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id, CancellationToken cancellationToken)
        {

            GetWorkItemByIdQuery query = new() { IdTask = id };
            var result = await _queryDispatcher.Dispatch<GetWorkItemByIdQuery, ApiResponse<WorkItemDto>>(query, cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<DeleteWorkItemCommand, ApiResponse<bool>>(new DeleteWorkItemCommand { IdTask = id }, cancellationToken);
            return result.Success ? Ok() : BadRequest(result);
        }
    }
}
