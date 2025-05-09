using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Commands.Products;
using SalesService.Contracts.Products;

namespace SalesService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // Simulate a delay to mimic a real-world scenario
            System.Threading.Thread.Sleep(1000);
            // Return a simple message
            return Ok("Product data retrieved successfully.");
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // Simulate a delay to mimic a real-world scenario
            System.Threading.Thread.Sleep(1000);
            // Return a simple message
            return Ok($"Product with ID {id} retrieved successfully.");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var command = new CreateProductCommand(request.Name, request.Description, request.Category, request.Price, request.Tax, request.ImageFile);
            var result = await _mediator.Send(command);

            return CreatedAtRoute(result.Id, result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] string product)
        {
            // Simulate a delay to mimic a real-world scenario
            System.Threading.Thread.Sleep(1000);
            // Return a simple message
            return Ok($"Product with ID {id} updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Simulate a delay to mimic a real-world scenario
            System.Threading.Thread.Sleep(1000);
            // Return a simple message
            return Ok($"Product with ID {id} deleted successfully.");
        }
    }
}
