using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartBusiness.Application.Commands.Users.Authentication.Login;
using SmartBusiness.Contracts.Requests.Users.Authentication;

namespace SmartBusiness.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
