using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartBusiness.Application.Commands.Authentication.Login;
using SmartBusiness.Application.Commands.UserCommands.Create;
using SmartBusiness.Contracts.Requests.Authentication;

namespace SmartBusiness.Api.Controllers
{
    [Route("users/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var command = new CreateUserCommand(request.Username, request.Email, request.Password);
            await _mediator.Send(command);

            return Created();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
