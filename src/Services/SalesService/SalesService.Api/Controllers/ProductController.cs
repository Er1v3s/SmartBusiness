using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Commands.Products;
using SalesService.Application.Queries.Products;

namespace SalesService.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand request)
        {
            var result = await _mediator.Send(request);

            return Created($"/api/products/{result.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductCommand request)
        {
            request.Id = id;
            await _mediator.Send(request);

            return Ok($"\"{request.Name}\" - updated successfully..");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var request = new DeleteProductCommand(id);
            await _mediator.Send(request);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var request = new GetProductsByIdQuery(id);
            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMany([FromQuery] GetProductsByParamsQuery request)
        {
            var result = await _mediator.Send(request);

            return Ok(result);
        }
    }
}
