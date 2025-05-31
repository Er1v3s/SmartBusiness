using AccountService.Application.Abstracts;
using AccountService.Contracts.Exceptions.Auth;
using AccountService.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Commands.Auth
{
    public record ResetPasswordCommand(string Token, string NewPassword) : IRequest;

    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
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

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ResetPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var query = _userRepository.GetQueryable();
            query = query.Where(u => u.ResetPasswordToken == request.Token);

            var user = await _userRepository.GetFilteredUserAsync(query, cancellationToken)
                       ?? throw new InvalidResetPasswordTokenException("Invalid reset password token.");

            if (user.ResetPasswordTokenExpiresAtUtc < DateTime.UtcNow)
                throw new InvalidResetPasswordTokenException("Reset password token has expired.");

            var passwordHash = _passwordHasher.HashPassword(new User(), request.NewPassword);
            user.PasswordHash = passwordHash;

            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiresAtUtc = null;

            await _userRepository.UpdateUserAsync(user);
        }
    }
}
