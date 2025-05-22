using MediatR;
using System.Security.Claims;
using AuthService.Api.Attributes;
using AuthService.Application.Commands.CompanyRole;
using AuthService.Contracts.Requests.CompanyRole;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/company/{companyId}/role")]
    [Authorize]
    public class CompanyRoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthorizeCompanyOwnerAttribute))]
        public async Task<IActionResult> GetCompanyRoles([FromRoute] string companyId, [FromQuery] GetCompanyRoleRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new GetCompanyRolesCommand(userId, companyId, request.Name);
            var result = await _mediator.Send(command);
            
            return Ok(result);
        }

        [HttpGet("{roleId}")]
        [TypeFilter(typeof(AuthorizeCompanyOwnerAttribute))]
        public async Task<IActionResult> GetCompanyRoleById([FromRoute] string companyId, [FromRoute] string roleId)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new GetCompanyRoleByIdCommand(userId, companyId, roleId);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthorizeCompanyOwnerAttribute))]
        public async Task<IActionResult> CreateCompanyRole([FromRoute] string companyId, [FromBody] CreateCompanyRoleRequest request)
        {
            var command = new CreateCompanyRoleCommand(request.UserId, companyId, request.Name);
            var result = await _mediator.Send(command);

            return Created($"/api/company/{companyId}/role/{result.Role.Id}", result);
        }

        [HttpPut("{roleId}")]
        [TypeFilter(typeof(AuthorizeCompanyOwnerAttribute))]
        public async Task<IActionResult> UpdateCompanyRole([FromRoute] string companyId, [FromRoute] string roleId, [FromBody] UpdateCompanyRoleRequest request)
        {
            var command = new UpdateCompanyRoleCommand(companyId, roleId, request.Name);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("{roleId}")]
        [TypeFilter(typeof(AuthorizeCompanyOwnerAttribute))]
        public async Task<IActionResult> DeleteCompanyRole([FromRoute] string companyId, [FromRoute] string roleId)
        {
            var command = new DeleteCompanyRoleCommand(companyId, roleId);
            await _mediator.Send(command);
            
            return NoContent();
        }
    }
}