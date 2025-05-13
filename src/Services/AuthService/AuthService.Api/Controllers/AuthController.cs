using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.Commands.Auth;
using AuthService.Contracts.Requests.Auth;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var command = new LoginUserCommand(request.Email, request.Password);

            //var result = await _mediator.Send(command);
            //return Ok(result);

            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            var command = new RegisterUserCommand(request.Username, request.Email, request.Password);
            await _mediator.Send(command);

            return Created();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var command = new RefreshTokenCommand(request.RefreshToken);

            //var result = await _mediator.Send(command);
            //return Ok(result);

            await _mediator.Send(command);
            return Ok();
        }
    }
}

