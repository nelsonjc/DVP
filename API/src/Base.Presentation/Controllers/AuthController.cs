using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Responses;

namespace TaskingSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher = queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthCommand command, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<AuthCommand, ApiResponse<UserDto>>(command, cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
