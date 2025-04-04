using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartBusiness.Application.Commands.Authentication.CreateUser;
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
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
