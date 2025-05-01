using MediatR;
using Microsoft.AspNetCore.Identity;
using AuthService.Application.Abstracts;
using AuthService.Contracts.Exceptions.Users;
using AuthService.Domain.Entities;

namespace AuthService.Application.Commands.Users.ChangePassword
{
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ChangeUserPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id, cancellationToken);

            if (user == null)
                throw new UserNotFoundException();

            if(_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword) != PasswordVerificationResult.Success)
                throw new InvalidPasswordException("Incorrect current password");

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.NewPassword) == PasswordVerificationResult.Success)
                throw new InvalidPasswordException("New password cannot be the same as the old password");

            var newPasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);

            await _userRepository.ChangeUserPasswordAsync(user, newPasswordHash, cancellationToken);

            return "Password changed successfully";
        }
    }
}
