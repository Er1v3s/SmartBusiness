using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AuthService.Domain.Entities;
using AuthService.Application.Abstracts;
using AuthService.Contracts.Exceptions.Auth;
using AuthService.Contracts.Exceptions.Users;

namespace AuthService.Application.Commands.Users.Authentication.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthTokenProcessor _authTokenProcessor;

        public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IAuthTokenProcessor authTokenProcessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authTokenProcessor = authTokenProcessor;
        }

        public async Task Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user == null)
                throw new UserNotFoundException();

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) !=
                PasswordVerificationResult.Success)
                throw new InvalidPasswordException("Incorrect password");

            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
            var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc =
                DateTime.UtcNow.AddHours(1); // Set the expiration time for the refresh token

            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

            await _userRepository.UpdateUserAsync(user);

            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken,
                refreshTokenExpirationDateInUtc);
        }

        public async Task RefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new RefreshTokenException("Refresh token is missing.");

            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null)
                throw new RefreshTokenException("Unable to retrieve user for refresh token.");

            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
                throw new RefreshTokenException("Refresh token is expired.");

            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
            var newRefreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc =
                DateTime.UtcNow.AddHours(1); // Set the expiration time for the refresh token

            user.RefreshToken = newRefreshTokenValue;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

            await _userRepository.UpdateUserAsync(user);

            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken,
                refreshTokenExpirationDateInUtc);
        }
    }
}
