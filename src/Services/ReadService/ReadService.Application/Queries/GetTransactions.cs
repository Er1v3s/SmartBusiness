using FluentValidation;
using MediatR;
using Shared.Abstracts;
using Shared.Entities;
using Shared.Validators;

namespace ReadService.Application.Queries
{
    public record GetTransactionsByParamsQuery(
        string? UserId,
        string? ProductId,
        int? Quantity,
        decimal? MinTotalAmount,
        decimal? MaxTotalAmount,
        int? MinTax,
        int? MaxTax,
        decimal? MinTotalAmountMinusTax,
        decimal? MaxTotalAmountMinusTax,
        DateTime? StartDateTime,
        DateTime? EndDateTime
        ) : IRequest<List<Transaction>>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetTransactionsRequestValidator : AbstractValidator<GetTransactionsByParamsQuery>
    {
        public GetTransactionsRequestValidator()
        {
            RuleFor(x => x.UserId)
                .Must(userId => Guid.TryParse(userId, out _))
                .WithMessage($"{nameof(Transaction.UserId)} must be a valid GUID")
                .When(x => !string.IsNullOrEmpty(x.UserId));

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage($"{nameof(Transaction.ProductId)} cannot be empty")
                .Must(Validator.BeValidNanoId)
                .WithMessage($"{nameof(Transaction.ProductId)} can only contain letters, numbers, '-' and '_'")
                .When(x => !string.IsNullOrEmpty(x.ProductId));

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage($"{nameof(GetTransactionsByParamsQuery.Quantity)} must be greater than 0.")
                .LessThanOrEqualTo(1000000)
                .WithMessage($"{nameof(GetTransactionsByParamsQuery.Quantity)} must be less or equal to 1,000,000")
                .When(x => x.Quantity != null);

            RuleFor(x => x.MinTotalAmount)
                .GreaterThan(0)
                .WithMessage($"{nameof(Transaction.TotalAmount)} must be greater than zero.")
                .LessThanOrEqualTo(1000000000)
                .WithMessage($"{nameof(Transaction.TotalAmount)} must be less than or equal to 1,000,000,000.")
                .PrecisionScale(12, 2, false)
                .WithMessage($"{nameof(Transaction.TotalAmount)} must have up to 2 decimal places and a maximum of 12 digits.")
                .When(x => x.MinTotalAmount != null);

            RuleFor(x => x.MaxTotalAmount)
                .GreaterThan(0)
                .WithMessage($"{nameof(Transaction.TotalAmount)} must be greater than zero.")
                .LessThanOrEqualTo(1000000000)
                .WithMessage($"{nameof(Transaction.TotalAmount)} must be less than or equal to 1,000,000,000.")
                .PrecisionScale(12, 2, false)
                .WithMessage($"{nameof(Transaction.TotalAmount)} must have up to 2 decimal places and a maximum of 12 digits.")
                .Must((query, maxTotalAmount) => maxTotalAmount >= query.MinTotalAmount)
                .WithMessage($"{nameof(Transaction.TotalAmount)} Max must be greater than or equal to Min.")
                .When(x => x.MaxTotalAmount != null);

            RuleFor(x => x.MinTax)
                .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(Transaction.Tax)} must be greater than or equal to zero.")
                .LessThanOrEqualTo(100)
                .WithMessage($"{nameof(Transaction.Tax)} must be less than or equal to 100.")
                .When(x => x.MinTax != null);

            RuleFor(x => x.MaxTax)
                .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(Transaction.Tax)} must be greater than or equal to zero.")
                .LessThanOrEqualTo(100)
                .WithMessage($"{nameof(Transaction.Tax)} must be less than or equal to 100.")
                .Must((query, maxTax) => maxTax >= query.MinTax)
                .WithMessage($"{nameof(Transaction.Tax)} Max must be greater than or equal to Min.")
                .When(x => x.MaxTax != null);

            RuleFor(x => x.MinTotalAmountMinusTax)
                .GreaterThan(0)
                .WithMessage($"{nameof(Transaction.TotalAmountMinusTax)} must be greater than zero.")
                .LessThanOrEqualTo(1000000000)
                .WithMessage($"{nameof(Transaction.TotalAmountMinusTax)} must be less than or equal to 1,000,000,000.")
                .PrecisionScale(12, 2, false)
                .WithMessage($"{nameof(Transaction.TotalAmountMinusTax)} must have up to 2 decimal places and a maximum of 12 digits.")
                .When(x => x.MinTotalAmountMinusTax != null);

            RuleFor(x => x.MaxTotalAmountMinusTax)
                .GreaterThan(0)
                .WithMessage($"{nameof(Transaction.TotalAmountMinusTax)} must be greater than zero.")
                .LessThanOrEqualTo(1000000000)
                .WithMessage($"{nameof(Transaction.TotalAmountMinusTax)} must be less than or equal to 1,000,000,000.")
                .PrecisionScale(12, 2, false)
                .WithMessage($"{nameof(Transaction.TotalAmountMinusTax)} must have up to 2 decimal places and a maximum of 12 digits.")
                .Must((query, maxTotalAmountMinusTax) => maxTotalAmountMinusTax >= query.MinTotalAmountMinusTax)
                .WithMessage($"{nameof(Transaction.TotalAmountMinusTax)} Max must be greater than or equal to Min.")
                .When(x => x.MaxTotalAmountMinusTax != null);

            RuleFor(x => x.StartDateTime)
                .LessThanOrEqualTo(x => x.EndDateTime)
                .WithMessage($"{nameof(GetTransactionsByParamsQuery.StartDateTime)} must be less than or equal to {nameof(GetTransactionsByParamsQuery.EndDateTime)}.")
                .When(x => x.StartDateTime.HasValue && x.EndDateTime.HasValue);

            RuleFor(x => x.EndDateTime)
                .GreaterThanOrEqualTo(x => x.StartDateTime)
                .WithMessage($"{nameof(GetTransactionsByParamsQuery.EndDateTime)} must be greater than or equal to {nameof(GetTransactionsByParamsQuery.StartDateTime)}.")
                .When(x => x.StartDateTime.HasValue && x.EndDateTime.HasValue);

            //RuleFor(x => x.CompanyId)
            //    .NotNull()
            //    .NotEmpty()
            //    .WithMessage($"{nameof(Transaction.CompanyId)} is required.");
        }
    }

    public class GetTransactionsRequestHandler : IRequestHandler<GetTransactionsByParamsQuery, List<Transaction>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionsRequestHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Transaction>> Handle(GetTransactionsByParamsQuery request, CancellationToken cancellationToken)
        {
            var query = _transactionRepository.GetQueryable();

            // IMPORTANT STATEMENT (User can only get transactions from declared company)
            query = query.Where(t => t.CompanyId == request.CompanyId);
            
            query = ApplyFilters(query, request);

            var transactions = await _transactionRepository.GetFilteredTransactionsAsync(query, cancellationToken);

            return transactions;
        }

        private IQueryable<Transaction> ApplyFilters(IQueryable<Transaction> query, GetTransactionsByParamsQuery request)
        {
            if (!string.IsNullOrEmpty(request.UserId))
                query = query.Where(t => t.UserId == request.UserId);

            if (!string.IsNullOrEmpty(request.ProductId))
                query = query.Where(t => t.ProductId == request.ProductId);

            if (request.Quantity.HasValue)
                query = query.Where(t => t.Quantity == request.Quantity.Value);

            if (request.MinTotalAmount.HasValue)
                query = query.Where(t => t.TotalAmount >= request.MinTotalAmount.Value);

            if (request.MaxTotalAmount.HasValue)
                query = query.Where(t => t.TotalAmount <= request.MaxTotalAmount.Value);

            if (request.MinTax.HasValue)
                query = query.Where(t => t.Tax >= request.MinTax.Value);

            if (request.MaxTax.HasValue)
                query = query.Where(t => t.Tax <= request.MaxTax.Value);

            if (request.MinTotalAmountMinusTax.HasValue)
                query = query.Where(t => t.TotalAmountMinusTax >= request.MinTotalAmountMinusTax.Value);

            if (request.MaxTotalAmountMinusTax.HasValue)
                query = query.Where(t => t.TotalAmountMinusTax <= request.MaxTotalAmountMinusTax.Value);

            if (request.StartDateTime.HasValue)
                query = query.Where(t => t.CreatedAt >= request.StartDateTime.Value);

            if (request.EndDateTime.HasValue)
                query = query.Where(t => t.CreatedAt <= request.EndDateTime.Value);

            return query;
        }
    }
}
