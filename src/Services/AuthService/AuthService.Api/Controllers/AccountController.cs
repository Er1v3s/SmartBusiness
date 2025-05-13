using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthService.Application.Commands.Account;
using AuthService.Contracts.Requests.Account;

namespace AuthService.Api.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var command = new UpdateUserCommand(id, request.Username, request.Email);

            //var result = await _mediator.Send(command);
            //return Ok(result);

            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
        {
            var command = new ChangeUserPasswordCommand(id, request.CurrentPassword, request.NewPassword);

            //var result = await _mediator.Send(command);
            //return Ok(result);

            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
