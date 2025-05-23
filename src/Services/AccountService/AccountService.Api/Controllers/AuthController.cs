using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountService.Application.Commands.Auth;
using AccountService.Contracts.Requests.Auth;

namespace AccountService.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
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
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new LogoutUserCommand(userId);
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
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["REFRESH_TOKEN"];
            var command = new RefreshTokenCommand(refreshToken);
            await _mediator.Send(command);

            return Ok();
        }
    }
}