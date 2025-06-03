using MediatR;
using AccountService.Application.Abstracts;
using AccountService.Contracts.Exceptions.Users;
using AccountService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Commands.Account
{
    public record DeleteUserCommand(Guid Id, string Password) : IRequest;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthTokenProcessor _authTokenProcessor;

        public DeleteUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IAuthTokenProcessor authTokenProcessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authTokenProcessor = authTokenProcessor;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id)
                ?? throw new UserNotFoundException();

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
                throw new InvalidPasswordException("Incorrect current password");

            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", "", DateTime.UtcNow.AddMinutes(-1));

            await _userRepository.DeleteUserAsync(user);
        }
    }
}
