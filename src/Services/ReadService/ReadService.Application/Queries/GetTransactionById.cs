using FluentValidation;
using MediatR;
using Shared.Abstracts;
using Shared.Entities;
using Shared.Exceptions;

namespace ReadService.Application.Queries
{
    public record GetTransactionByIdQuery(string TransactionId) : IRequest<Transaction>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetTransactionByIdQueryValidator : AbstractValidator<GetTransactionByIdQuery>
    {
        public GetTransactionByIdQueryValidator()
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

    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Transaction>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(request.TransactionId);
            if (transaction == null)
                throw new NotFoundException($"Transaction with ID {request.TransactionId} not found.");

            if (transaction.CompanyId != request.CompanyId)
                throw new ForbiddenException(
                    "You are not allowed to access this transaction from a different company.");

            return transaction;
        }
    }
}
