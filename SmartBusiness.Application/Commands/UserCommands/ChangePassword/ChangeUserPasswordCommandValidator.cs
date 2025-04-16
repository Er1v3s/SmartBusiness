using FluentValidation;

namespace SmartBusiness.Application.Commands.UserCommands.ChangePassword
{
    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            RuleFor(x => x.NewPassword)
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
