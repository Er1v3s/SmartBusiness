using MediatR;
using AccountService.Application.Abstracts;
using AccountService.Contracts.Exceptions.Auth;

namespace AccountService.Application.Commands.Auth
{
    public record RefreshTokenCommand(string? RefreshToken) : IRequest;

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenProcessor _authTokenProcessor;

        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IAuthTokenProcessor authTokenProcessor)
        {
            _userRepository = userRepository;
            _authTokenProcessor = authTokenProcessor;
        }

        public async Task Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                throw new RefreshTokenException("Refresh token is missing.");

            var decodedRefreshToken = Uri.UnescapeDataString(request.RefreshToken);

            var query = _userRepository.GetQueryableIncludingProperties();
            query = query.Where(u => u.RefreshToken == request.RefreshToken);

            var user = await _userRepository.GetFilteredUserAsync(query, cancellationToken)
                       ?? throw new RefreshTokenException("Unable to retrieve user for refresh token.");

            if (user.RefreshToken != decodedRefreshToken || user.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
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
