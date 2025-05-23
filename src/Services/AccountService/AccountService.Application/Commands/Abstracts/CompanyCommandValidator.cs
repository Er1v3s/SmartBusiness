using FluentValidation;
using AccountService.Domain.Entities;

namespace AccountService.Application.Commands.Abstracts
{
    public abstract class CompanyCommandValidator<T> : AbstractValidator<T> where T : CompanyCommand
    {
        protected CompanyCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Company.Name)} name is required.")
                .MinimumLength(3)
                .WithMessage($"{nameof(Company.Name)} name must be at least 3 characters long.")
                .MaximumLength(100)
                .WithMessage($"{nameof(Company.Name)} name cannot be longer than 100 characters.")
                .Matches(@"^[a-zA-Z0-9 _\-\(\)]*$")
                .WithMessage($"{nameof(Company.Name)} can only contain letters, digits, space, underscore (_), hyphen (-), and parentheses (() ).");
        }
    }
}
