using FluentValidation;
using MediatR;
using Shared.Contracts;
using Shared.Exceptions;
using WriteService.Application.Abstracts;
using Shared.Abstracts;
using Shared.Entities;

namespace WriteService.Application.Commands.Transactions
{
    public record DeleteTransactionCommand(string TransactionId) : IRequest<Unit>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
    {
        public DeleteTransactionCommandValidator()
        {
            RuleFor(x => x.TransactionId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Transaction.Id)} is required.");

            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Transaction.CompanyId)} is required.");
        }
    }

    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Unit>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventBus _eventBus;

        public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository, IEventBus eventBus)
        {
            _transactionRepository = transactionRepository;
            _eventBus = eventBus;
        }

        public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var existingTransaction = await _transactionRepository.GetTransactionByIdAsync(request.TransactionId);
            if (existingTransaction == null)
                throw new NotFoundException($"Transaction with id {request.TransactionId} not found");

            if (existingTransaction.CompanyId != request.CompanyId)
                throw new ForbiddenException("You are not able to delete transaction from other company than you are register in");

            await _transactionRepository.DeleteAsync(existingTransaction.Id);

            await _eventBus.PublishAsync(new TransactionDeletedEvent(existingTransaction.Id), cancellationToken);

            return Unit.Value;
        }
    }
}
