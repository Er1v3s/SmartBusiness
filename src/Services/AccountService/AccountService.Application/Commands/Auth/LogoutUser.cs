using MediatR;
using AccountService.Application.Abstracts;
using AccountService.Contracts.Exceptions.Users;

namespace AccountService.Application.Commands.Auth
{
    public record LogoutUserCommand(Guid UserId) : IRequest;

    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenProcessor _authTokenProcessor;

        public LogoutUserCommandHandler(IUserRepository userRepository, IAuthTokenProcessor authTokenProcessor)
        {
            _userRepository = userRepository;
            _authTokenProcessor = authTokenProcessor;
        }
        public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId)
                ?? throw new UserNotFoundException();

            var expirationTimeInUtc = DateTime.UtcNow.AddHours(-1);

            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", "", expirationTimeInUtc);

            user.RefreshToken = null;
            user.RefreshTokenExpiresAtUtc = null;

            await _userRepository.UpdateUserAsync(user);
        }
    }
}
