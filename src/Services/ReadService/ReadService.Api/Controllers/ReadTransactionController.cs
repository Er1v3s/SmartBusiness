using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadService.Application.Queries;

namespace ReadService.Api.Controllers
{
    [Route("api/read/transactions")]
    [ApiController]
    [Authorize]
    public class ReadTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReadTransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(string id)
        {
            var request = new GetTransactionByIdQuery(id);
            var result = await _mediator.Send(request);

            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] GetTransactionsByParamsQuery request)
        {
            var result =  await _mediator.Send(request);

            return Ok(result);
        }
    }
}
