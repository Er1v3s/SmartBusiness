using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Commands.Products;
using SalesService.Contracts.Requests;

namespace SalesService.Api.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var command = new CreateProductCommand(request.Name, request.Description, request.Category, request.Price, request.Tax, request.ImageFile);
            var result = await _mediator.Send(command);

            return Created($"/api/product/{result.Id}", result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] Guid? id,
            [FromQuery] string? name,
            [FromQuery] string? category,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var command = new GetProductsCommand(id, name, category, minPrice, maxPrice);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            var command = new UpdateProductCommand(id, request.Name, request.Description, request.Category, request.Price, request.Tax, request.ImageFile);
            var result = await _mediator.Send(command);

            return Ok($"\"{result.Name}\" - updated successfully..");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProductCommand(id);
            var result = await _mediator.Send(command);

            return Ok($"\"{result.Name}\" - deleted successfully.");
        }
    }
}
