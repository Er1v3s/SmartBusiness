using AuthService.Application.Abstracts;
using AuthService.Contracts.Exceptions.Auth;
using MediatR;

namespace AuthService.Application.Commands.Auth
{
    public record RefreshTokenCommand(string? RefreshToken) : IRequest;

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenProcessor _authTokenProcessor;

        public RefreshTokenCommandHandler(IUserRepository userRepository, IAuthTokenProcessor authTokenProcessor)
        {
            _userRepository = userRepository;
            _authTokenProcessor = authTokenProcessor;
        }

        public async Task Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                throw new RefreshTokenException("Refresh token is missing.");

            var user = await _userRepository.GetUserByRefreshTokenAsync(request.RefreshToken)
                       ?? throw new RefreshTokenException("Unable to retrieve user for refresh token.");

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
                throw new RefreshTokenException("Refresh token is expired.");

            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
            var newRefreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc =
                DateTime.UtcNow.AddHours(12); // Set the expiration time for the refresh token

            user.RefreshToken = newRefreshTokenValue;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;
            await _userRepository.UpdateUserAsync(user);

            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken,
                refreshTokenExpirationDateInUtc);
        }
    }
}
