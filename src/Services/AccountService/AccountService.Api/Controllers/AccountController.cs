using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountService.Application.Commands.Account;
using AccountService.Contracts.Requests.Account;

namespace AccountService.Api.Controllers
{   
    [ApiController]
    [Route("api/account")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUser()
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new GetUserCommand(userId);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new UpdateUserCommand(userId, request.Username, request.Email);
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new ChangeUserPasswordCommand(userId, request.CurrentPassword, request.NewPassword);
            await _mediator.Send(command);

            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser()
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new DeleteUserCommand(userId);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
