using MediatR;
using Shared.Abstracts;
using Shared.Entities;

namespace ReadService.Application.Requests
{
    public record GetTransactionsRequest : IRequest<List<Transaction>>;

    public class GetTransactionsRequestHandler : IRequestHandler<GetTransactionsRequest, List<Transaction>>
    {
        private readonly ITransactionRepository _transactionRepository;
        public GetTransactionsRequestHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<List<Transaction>> Handle(GetTransactionsRequest request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetAllAsync();
            //return transactions.Select(t => new TransactionDto
            //{
            //    Id = t.Id,
            //    Amount = t.Amount,
            //    Date = t.Date,
            //    UserId = t.UserId,
            //    CompanyId = t.CompanyId
            //}).ToList();

            return transactions;
        }
    }
}
