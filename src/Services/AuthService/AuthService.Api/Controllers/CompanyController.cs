using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthService.Application.Commands.Companies;
using AuthService.Contracts.Requests.Companies;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompany([FromQuery] GetCompanyRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new GetCompanyCommand(userId, request.Id, request.Name);
            var result = await _mediator.Send(command);
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new CreateCompanyCommand(userId, request.Name);
            var result = await _mediator.Send(command);
            
            return Created($"/api/company/{result.Id}", result.Id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateCompany(string id, [FromBody] UpdateCompanyRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new UpdateCompanyCommand(userId, id, request.Name);
            var result = await _mediator.Send(command);
            
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new DeleteCompanyCommand(userId, id);
            await _mediator.Send(command);
            
            return NoContent();
        }
    }
}