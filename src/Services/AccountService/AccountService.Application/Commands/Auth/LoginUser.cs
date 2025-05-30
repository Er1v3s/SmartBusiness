using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using AccountService.Application.Abstracts;
using AccountService.Contracts.DTOs;
using AccountService.Contracts.Exceptions.Users;
using AccountService.Domain.Entities;

namespace AccountService.Application.Commands.Auth
{
    public record LoginUserCommand(string Email, string Password) : IRequest<JwtDto>;

    public class LoginUserCommandValidator : AbstractValidator<User>
    {
        // TO CHANGE
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

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, JwtDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthTokenProcessor _authTokenProcessor;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IAuthTokenProcessor authTokenProcessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authTokenProcessor = authTokenProcessor;
        }

        public async Task<JwtDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var query = _userRepository.GetQueryableIncludingProperties();
            query = query.Where(u => u.Email == request.Email);

            var user = await _userRepository.GetFilteredUserAsync(query, cancellationToken)
                ?? throw new UserNotFoundException();

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) !=
                PasswordVerificationResult.Success)
                throw new InvalidPasswordException("Incorrect password");

            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
            var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc =
                DateTime.UtcNow.AddHours(12); // Set the expiration time for the refresh token

            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

            await _userRepository.UpdateUserAsync(user);

            //_authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken,
                refreshTokenExpirationDateInUtc);

            var jwtDto = new JwtDto
            {
                JwtToken = jwtToken,
                ExpirationDateInUtc = expirationDateInUtc,
            };

            return jwtDto;
        }
    }
}
