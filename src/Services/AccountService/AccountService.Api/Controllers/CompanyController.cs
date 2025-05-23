using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountService.Application.Commands.Companies;
using AccountService.Contracts.Requests.Companies;
using AccountService.Api.Attributes;

namespace AccountService.Api.Controllers
{
    [ApiController]
    [Route("api/company")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies([FromQuery] GetCompanyRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new GetCompanyCommand(userId, request.Name);
            var result = await _mediator.Send(command);
            
            return Ok(result);
        }

        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetCompanyById([FromRoute] string companyId)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new GetCompanyByIdCommand(userId, companyId);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new CreateCompanyCommand(userId, request.Name);
            var result = await _mediator.Send(command);
            
            return Created($"/api/company?Id={result.Id}", result);
        }

        [HttpPut("{companyId}")]
        [TypeFilter(typeof(AuthorizeCompanyOwnerAttribute))]
        public async Task<IActionResult> UpdateCompany([FromRoute] string companyId, [FromBody] UpdateCompanyRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new UpdateCompanyCommand(userId, companyId, request.Name!);
            var result = await _mediator.Send(command);
            
            return Ok(result);
        }

        [HttpDelete("{companyId}")]
        [TypeFilter(typeof(AuthorizeCompanyOwnerAttribute))]
        public async Task<IActionResult> DeleteCompany([FromRoute] string companyId)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var command = new DeleteCompanyCommand(userId, companyId);
            await _mediator.Send(command);
            
            return NoContent();
        }
    }
}