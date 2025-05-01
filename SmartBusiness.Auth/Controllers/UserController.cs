using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.Commands.Users.ChangePassword;
using AuthService.Application.Commands.Users.Create;
using AuthService.Application.Commands.Users.Delete;
using AuthService.Application.Commands.Users.Update;
using AuthService.Contracts.Requests.Users;

namespace AuthService.Api.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateRequest request)
        {
            var command = new CreateUserCommand(request.Username, request.Email, request.Password);
            await _mediator.Send(command);

            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRequest request)
        {
            var command = new UpdateUserCommand(id, request.Username, request.Email);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
        {
            var command = new ChangeUserPasswordCommand(id, request.CurrentPassword, request.NewPassword);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
