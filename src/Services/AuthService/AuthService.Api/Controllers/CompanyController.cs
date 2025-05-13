using AuthService.Application.Commands.Companies;
using AuthService.Contracts.Requests.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var command = new GetCompanyCommand(request.Id, request.Name);
            var result = await _mediator.Send(command);
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
        {
            var command = new CreateCompanyCommand(request.Name);
            var result = await _mediator.Send(command);
            
            return CreatedAtRoute($"/api/company/{result.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(string id, [FromBody] UpdateCompanyRequest request)
        {
            var command = new UpdateCompanyCommand(id, request.Name);
            var result = await _mediator.Send(command);
            
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            var command = new DeleteCompanyCommand(id);
            var result = await _mediator.Send(command);
            
            return NoContent();
        }
    }
}