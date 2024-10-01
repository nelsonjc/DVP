using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.CQRS.Queries;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Responses;

namespace TaskingSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher = queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetUserFilterQuery query, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetUserFilterQuery, ApiResponse<UserDto[]>>(query, cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<CreateUserCommand, ApiResponse<Guid>>(command, cancellationToken);
            return result.Success ? CreatedAtAction(nameof(GetUserById), new { id = result.Data }, result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            var result = await _commandDispatcher.Dispatch<UpdateUserCommand, ApiResponse<bool>>(command, cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
        {

            GetUserByIdQuery query = new() { IdUser = id };
            var result = await _queryDispatcher.Dispatch<GetUserByIdQuery, ApiResponse<UserDto>>(query, cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<DeleteUserCommand, ApiResponse<bool>>(new DeleteUserCommand { IdUser = id }, cancellationToken);
            return result.Success ? Ok() : BadRequest(result);
        }

        [HttpGet("GetByRolName/{rolName}")]
        [Authorize(Roles = "Administrador,Supervisor")]
        public async Task<IActionResult> GetUserById(string rolName, CancellationToken cancellationToken)
        {

            GetUserByRolQuery query = new() { RolName = rolName };
            var result = await _queryDispatcher.Dispatch<GetUserByRolQuery, ApiResponse<IEnumerable<UserDto>>>(query, cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-all-role")]
        [Authorize(Roles = "Administrador,Supervisor")]
        public async Task<IActionResult> GetAllRole([FromQuery] GetRoleFilterQuery filters, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetRoleFilterQuery, ApiResponse<IEnumerable<RoleDto>>>(filters, cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
