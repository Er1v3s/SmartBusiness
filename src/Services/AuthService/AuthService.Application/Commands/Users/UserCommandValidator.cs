using FluentValidation;
using AuthService.Domain.Entities;

namespace AuthService.Application.Commands.Users
{
    public abstract class UserCommandValidator<T> : AbstractValidator<T> where T : UserCommand
    {
        protected UserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(User.Username)} is required.")
                .MinimumLength(3)
                .WithMessage($"{nameof(User.Username)} must be at least 3 characters long.")
                .MaximumLength(50)
                .WithMessage($"{nameof(User.Username)} cannot be longer then 50 characters.");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(User.Email)} is required.")
                .MinimumLength(5)
                .WithMessage($"{nameof(User.Email)} must be at least 5 characters long.")
                .MaximumLength(100)
                .WithMessage($"{nameof(User.Email)} cannot be longer then 100 characters.")
                .EmailAddress()
                .WithMessage($"{nameof(User.Email)} must be in correct format. 'example@example.com'")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage($"{nameof(User.Email)} must be in correct format. 'example@example.com'");
        }
    }
}
