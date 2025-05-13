using FluentValidation;
using AuthService.Domain.Entities;

namespace AuthService.Application.Commands.Abstracts
{
    public abstract class UserCommandValidator<T> : AbstractValidator<T> where T : UserCommand
    {
        protected UserCommandValidator()
        {
            RuleFor(x => x.Username)
                .MinimumLength(3)
                .WithMessage($"{nameof(User.Username)} must be at least 3 characters long.")
                .When(x => !string.IsNullOrEmpty(x.Username))
                .MaximumLength(50)
                .WithMessage($"{nameof(User.Username)} cannot be longer then 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.Username));

            RuleFor(x => x.Email)
                .MinimumLength(5)
                .WithMessage($"{nameof(User.Email)} must be at least 5 characters long.")
                .When(x => !string.IsNullOrEmpty(x.Email))
                .MaximumLength(100)
                .WithMessage($"{nameof(User.Email)} cannot be longer then 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Email))
                .EmailAddress()
                .WithMessage($"{nameof(User.Email)} must be in correct format. 'example@example.com'")
                .When(x => !string.IsNullOrEmpty(x.Email))
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage($"{nameof(User.Email)} must be in correct format. 'example@example.com'")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}
