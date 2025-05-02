using MediatR;
using Microsoft.AspNetCore.Identity;
using AuthService.Domain.Entities;
using AuthService.Application.Abstracts;
using AuthService.Contracts.Exceptions.Users;

namespace AuthService.Application.Commands.Users.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var isEmailInUse = await _userRepository.IsEmailInUseAsync(request.Email, cancellationToken);
            if(isEmailInUse)
                throw new UserAlreadyExistsException("Email is already in use");

            var passwordHash = _passwordHasher.HashPassword(new User(), request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash
            };

            await _userRepository.AddUserAsync(user, cancellationToken);

            return user.Username;
        }
    }
}
