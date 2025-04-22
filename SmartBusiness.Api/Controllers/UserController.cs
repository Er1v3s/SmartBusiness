using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartBusiness.Application.Commands.Users.ChangePassword;
using SmartBusiness.Application.Commands.Users.Create;
using SmartBusiness.Application.Commands.Users.Delete;
using SmartBusiness.Application.Commands.Users.Update;
using SmartBusiness.Contracts.Requests.Users;

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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateRequest request)
        {
            var command = new CreateUserCommand(request.Username, request.Email, request.Password);
            await _mediator.Send(command);

            return Created();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateRequest request)
        {
            var command = new UpdateUserCommand(request.Id, request.Username, request.Email);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var command = new ChangeUserPasswordCommand(request.Id, request.CurrentPassword, request.NewPassword);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
        {
            var command = new DeleteUserCommand(request.Id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
