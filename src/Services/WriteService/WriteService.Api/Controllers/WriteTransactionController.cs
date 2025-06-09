using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WriteService.Application.Commands.Transactions;

namespace WriteService.Api.Controllers
{
    [Route("api/write/transactions")]
    [ApiController]
    [Authorize]
    public class WriteTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WriteTransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionCommand request)
        {
            var result =  await _mediator.Send(request);

            return Created($"/api/write/transactions/{result.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTransactionCommand request)
        {
            request.TransactionId = id;
            await _mediator.Send(request);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var request = new DeleteTransactionCommand(id);
            await _mediator.Send(request);

            return NoContent();
        }
    }
}
