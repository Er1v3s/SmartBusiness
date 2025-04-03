using FluentValidation;
using SmartBusiness.Domain.Entities;

namespace SmartBusiness.Application.Commands.Auth.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(User.Email)} is required.")
                .EmailAddress()
                .WithMessage($"{nameof(User.Username)} email format.");

            RuleFor(x => x.Username)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(User.Username)} is required.")
                .MinimumLength(3)
                .WithMessage($"{nameof(User.Username)} must be at least 3 characters long.")
                .MaximumLength(50)
                .WithMessage($"{nameof(User.Username)} cannot be longer then 50 characters.");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
        }
    }
}