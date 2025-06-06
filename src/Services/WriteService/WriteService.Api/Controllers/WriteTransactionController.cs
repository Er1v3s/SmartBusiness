using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WriteService.Application.Commands.Transactions;

namespace WriteService.Api.Controllers
{
    [Route("api/write/transactions")]
    [ApiController]
    public class WriteTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WriteTransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            var result =  await _mediator.Send(command);

            return CreatedAtAction(nameof(CreateTransaction), new { id = result }, command);
        }
    }
}
