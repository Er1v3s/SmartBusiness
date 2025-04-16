using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBusiness.Application.Commands.UserCommands.ChangePassword;
using SmartBusiness.Application.Commands.UserCommands.Delete;
using SmartBusiness.Application.Commands.UserCommands.Update;
using SmartBusiness.Contracts.Requests.Authentication;

namespace SmartBusiness.Api.Controllers
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

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            var command = new UpdateUserCommand(request.Id, request.Username, request.Email);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordRequest request)
        {
            var command = new ChangeUserPasswordCommand(request.Id, request.CurrentPassword, request.NewPassword);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteUserRequest request)
        {
            var command = new DeleteUserCommand(request.Id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
