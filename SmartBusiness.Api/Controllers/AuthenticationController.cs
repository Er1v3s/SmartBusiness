using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartBusiness.Application.Commands.Authentication.ChangeUserPassword;
using SmartBusiness.Application.Commands.Authentication.CreateUser;
using SmartBusiness.Application.Commands.Authentication.DeleteUser;
using SmartBusiness.Application.Commands.Authentication.UpdateUser;
using SmartBusiness.Contracts.Requests.Authentication;

namespace SmartBusiness.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var command = new CreateUserCommand(request.Username, request.Email, request.Password);
            await _mediator.Send(command);

            return Created();
        }


        [HttpPut("update-user")]
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

        [HttpDelete("delete-user")]
        public async Task<IActionResult> Delete([FromBody] DeleteUserRequest request)
        {
            var command = new DeleteUserCommand(request.Id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
