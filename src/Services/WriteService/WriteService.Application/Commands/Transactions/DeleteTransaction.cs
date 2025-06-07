using MediatR;
using Shared.Abstracts;
using Shared.Contracts;
using Shared.Exceptions;
using WriteService.Application.Abstracts;

namespace WriteService.Application.Commands.Transactions
{
    public record DeleteTransactionCommand(string Id) : IRequest<bool>;

    public class DeleteTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, bool>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventBus _eventBus;

        public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository, IEventBus eventBus)
        {
            _transactionRepository = transactionRepository;
            _eventBus = eventBus;
        }

        public async Task<bool> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetByIdAsync(request.Id);
            if (transaction == null)
                throw new NotFoundException($"Transaction {transaction} not found");

            await _transactionRepository.DeleteAsync(transaction.Id);

            await _eventBus.PublishAsync(new TransactionDeletedEvent(transaction.Id), cancellationToken);

            return true;
        }
    }
}
