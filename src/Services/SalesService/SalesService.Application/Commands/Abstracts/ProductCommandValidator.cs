using FluentValidation;
using SalesService.Domain.Entities;

namespace SalesService.Application.Commands.Abstracts
{
    public abstract class ProductCommandValidator<T> : AbstractValidator<T> where T : ProductCommand
    {
        protected ProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Name)} is required.")
                .MinimumLength(3)
                .WithMessage($"{nameof(Product.Name)} must be at least 3 characters long.")
                .MaximumLength(100)
                .WithMessage($"{nameof(Product.Name)} cannot be longer than 100 characters.");
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Description)} is required")
                .MinimumLength(3)
                .WithMessage($"{nameof(Product.Description)} must be at least 3 characters long.")
                .MaximumLength(500)
                .WithMessage($"{nameof(Product.Description)} cannot be longer than 500 characters.");
            RuleFor(x => x.Category)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Category)} is required.")
                .MinimumLength(3)
                .WithMessage($"{nameof(Product.Category)} must be at least 3 characters long.")
                .MaximumLength(50)
                .WithMessage($"{nameof(Product.Category)} cannot be longer than 50 characters.");
            RuleFor(x => x.Price)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Price)} is required.")
                .GreaterThan(0)
                .WithMessage($"{nameof(Product.Price)} must be greater than 0.")
                .PrecisionScale(9, 2, false)
                .WithMessage($"{nameof(Product.Price)} must have up to 2 decimal places and a maximum of 7 digits.");
            RuleFor(x => x.Tax)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Tax)} is required.")
                .InclusiveBetween(0, 100)
                .WithMessage($"{nameof(Product.Tax)} must be greater than 0 and less than 100.");
        }
    }   
}
