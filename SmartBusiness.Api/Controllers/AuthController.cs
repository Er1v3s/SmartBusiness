using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartBusiness.Application.Commands.Auth.RegisterUser;
using SmartBusiness.Contracts.Requests.Auth;

namespace SmartBusiness.Api.Controllers
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var command = new RegisterUserCommand(request.Username, request.Email, request.Password);

            var user = await _mediator.Send(command);
            return Ok(user);
        }
    }
}
