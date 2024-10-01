using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.CQRS.Queries;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Responses;

namespace TaskingSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MasterController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher = queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        [HttpGet("GetStatus")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetStatus([FromQuery] GetStatusAllQuery request,CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetStatusAllQuery, ApiResponse<IEnumerable<StatusDto>>>(request, cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
