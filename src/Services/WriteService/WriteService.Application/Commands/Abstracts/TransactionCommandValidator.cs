using FluentValidation;
using Shared.Entities;

namespace WriteService.Application.Commands.Abstracts
{
    public class TransactionCommandValidator<T> : AbstractValidator<T> where T : TransactionCommand
    {
        public TransactionCommandValidator()
        {
            RuleFor(x => x.ItemId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Transaction.ItemId)} is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage($"{nameof(Transaction.Quantity)} must be greater than zero.")
                .LessThanOrEqualTo(1000000)
                .WithMessage($"{nameof(Transaction.Quantity)} must be less than or equal to 1,000,000.");

            RuleFor(x => x.ItemType)
                .NotEmpty()
                .WithMessage($"{nameof(Transaction.ItemType)} cannot be empty.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0)
                .WithMessage($"{nameof(Transaction.TotalAmount)} must be greater than zero.")
                .LessThanOrEqualTo(1000000000)
                .WithMessage($"{nameof(Transaction.TotalAmount)} must be less than or equal to 1,000,000,000.")
                .PrecisionScale(12, 2, false)
                .WithMessage($"{nameof(Transaction.TotalAmount)} must have up to 2 decimal places and a maximum of 12 digits.");

            RuleFor(x => x.Tax)
                .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(Transaction.Tax)} must be greater than or equal to zero.")
                .LessThanOrEqualTo(100)
                .WithMessage($"{nameof(Transaction.Tax)} must be lass than or equal to 100");
        }
    }
}
