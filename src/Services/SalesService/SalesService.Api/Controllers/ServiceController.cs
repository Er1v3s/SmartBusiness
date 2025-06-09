using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Commands.Services;
using SalesService.Application.Queries.Services;

namespace SalesService.Api.Controllers
{
    [ApiController]
    [Route("api/services")]
    [Authorize]
    public class ServiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceCommand request)
        {
            var result = await _mediator.Send(request);

            return Created($"/api/products/{result.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateServiceCommand request)
        {
            request.ServiceId = id;
            await _mediator.Send(request);

            return Ok($"\"{request.Name}\" - updated successfully..");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var request = new DeleteServiceCommand(id);
            await _mediator.Send(request);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var query = new GetServicesByIdQuery(id);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetServicesByParamsQuery request)
        {
            var result = await _mediator.Send(request);

            return Ok(result);
        }
    }
}
