using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadService.Application.Requests;

namespace ReadService.Api.Controllers
{
    [Route("api/read/transactions")]
    [ApiController]
    public class ReadTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReadTransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetTransactions([FromBody] GetTransactionsRequest request)
        {
            var result =  await _mediator.Send(request);

            return Ok(result);
        }
    }
}
