using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using AuthService.Application.Abstracts;
using AuthService.Contracts.Exceptions.Users;
using AuthService.Domain.Entities;

namespace AuthService.Application.Commands.Account
{
    public record ChangeUserPasswordCommand(Guid Id, string CurrentPassword, string NewPassword) : IRequest;

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

    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ChangeUserPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id, cancellationToken)
                ?? throw new UserNotFoundException();
            
            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword) != PasswordVerificationResult.Success)
                throw new InvalidPasswordException("Incorrect current password");

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.NewPassword) == PasswordVerificationResult.Success)
                throw new InvalidPasswordException("New password cannot be the same as the old password");

            var newPasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);

            await _userRepository.ChangeUserPasswordAsync(user, newPasswordHash, cancellationToken);
        }
    }
}
