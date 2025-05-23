using FluentValidation;

namespace AccountService.Application.Commands.Abstracts
{
    public class UserCompanyRoleCommandValidator<T> : AbstractValidator<T> where T : UserCompanyRoleCommand
    {
        protected UserCompanyRoleCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MinimumLength(3)
                .WithMessage("Name must be greater than 3")
                .MaximumLength(50)
                .WithMessage("Name must be less than 50");
        }
    }
}

