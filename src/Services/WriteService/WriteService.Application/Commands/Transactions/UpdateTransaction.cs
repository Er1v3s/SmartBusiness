using MediatR;
using WriteService.Application.Abstracts;
using WriteService.Domain.Entities;

namespace WriteService.Application.Commands.Transactions
{
    public record UpdateTransactionCommand(
        string Id,
        string CompanyId,
        string UserId,
        string ProductId,
        int Quantity,
        decimal TotalAmount
    ) : IRequest<bool>;

    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, bool>
    {
        private readonly ITransactionRepository _transactionRepository;

        public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<bool> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                Id = request.Id,
                CompanyId = request.CompanyId,
                UserId = request.UserId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                TotalAmount = request.TotalAmount,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.UpdateAsync(request.Id, transaction);

            return true;
        }
    }
}
