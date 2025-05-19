using MediatR;
using System.Security.Claims;
using AuthService.Application.Commands.CompanyRole;
using AuthService.Contracts.Requests.CompanyRole;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/company/role")]
    [Authorize]
    public class CompanyRoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanyRoles([FromQuery] GetCompanyRoleRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new GetCompanyRoleCommand(userId, request.Id, request.Name);
            var result = await _mediator.Send(command);
            
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateCompanyRole([FromBody] CreateCompanyRoleRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new CreateCompanyRoleCommand(userId, request.Id, request.Name);
            var result = await _mediator.Send(command);
            
            return Created($"/api/company/role?Id={result.Id}", result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateCompanyRole(string id, [FromBody] UpdateCompanyRoleRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new UpdateCompanyRoleCommand(userId, id, request.Name);
            var result = await _mediator.Send(command);
            
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteCompanyRole(string id)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new DeleteCompanyRoleCommand(userId, id);
            await _mediator.Send(command);
            
            return NoContent();
        }
    }
}