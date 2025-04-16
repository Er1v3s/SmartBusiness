using FluentValidation;
using SmartBusiness.Domain.Entities;

namespace SmartBusiness.Application.Commands.Authentication.Login
{

    // TO CHANGE
    public class LoginUserCommandValidator : AbstractValidator<User>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("Email is required.");

            RuleFor(x => x.PasswordHash)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}