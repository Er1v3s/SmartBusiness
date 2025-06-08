using FluentValidation;
using SalesService.Domain.Entities;

namespace SalesService.Application.Commands.Abstracts
{
    public abstract class ServiceCommandValidator<T> : AbstractValidator<T> where T : ServiceCommand
    {
        protected ServiceCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.Name)} is required.")
                .MinimumLength(3)
                .WithMessage($"{nameof(Service.Name)} must be at least 3 characters long.")
                .MaximumLength(100)
                .WithMessage($"{nameof(Service.Name)} cannot be longer than 100 characters.");
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.Description)} is required")
                .MinimumLength(3)
                .WithMessage($"{nameof(Service.Description)} must be at least 3 characters long.")
                .MaximumLength(500)
                .WithMessage($"{nameof(Service.Description)} cannot be longer than 500 characters.");
            RuleFor(x => x.Category)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.Category)} is required.")
                .MinimumLength(3)
                .WithMessage($"{nameof(Service.Category)} must be at least 3 characters long.")
                .MaximumLength(100)
                .WithMessage($"{nameof(Service.Category)} cannot be longer than 50 characters.");
            RuleFor(x => x.Price)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.Price)} is required.")
                .GreaterThan(0)
                .WithMessage($"{nameof(Service.Price)} must be greater than 0.")
                .PrecisionScale(7, 2, false)
                .WithMessage($"{nameof(Service.Price)} must have up to 2 decimal places and a maximum of 5 digits.");
            RuleFor(x => x.Tax)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.Tax)} is required.")
                .InclusiveBetween(0, 100)
                .WithMessage($"{nameof(Service.Tax)} must be greater than 0 and less than 100.");
            RuleFor(x => x.Duration)
                .InclusiveBetween(0, 600)
                .WithMessage($"{nameof(Service.Duration)} must be greater than 0 and less than 600");
        }
    }   
}
