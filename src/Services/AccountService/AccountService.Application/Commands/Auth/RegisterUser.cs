using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using AccountService.Application.Abstracts;
using AccountService.Contracts.Exceptions.Users;
using AccountService.Domain.Entities;
using AccountService.Application.Commands.Abstracts;

namespace AccountService.Application.Commands.Auth
{
    public record RegisterUserCommand(string Username, string Email, string Password)
        : UserCommand(Username, Email), IRequest;

    public class RegisterUserCommandValidator : UserCommandValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(User.Username)} is required.");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(User.Email)} is required.");

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

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var isEmailInUse = await _userRepository.IsEmailInUseAsync(request.Email);
            if (isEmailInUse)
                throw new UserAlreadyExistsException("Email is already in use");

            var passwordHash = _passwordHasher.HashPassword(new User(), request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash
            };

            await _userRepository.AddUserAsync(user);
            
        }
    }
}
