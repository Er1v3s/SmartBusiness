using FluentValidation;

namespace SmartBusiness.Application.Commands.Authentication.CreateUser
{
    public class CreateUserCommandValidator : UserCommandValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(50)
                .WithMessage("Password cannot be longer than 50 characters.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
        }
    }
}