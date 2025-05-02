using AuthService.Application.Abstracts;
using AuthService.Application.Commands.Users.Update;
using AuthService.Contracts.DataTransferObjects;
using AuthService.Contracts.Exceptions.Users;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Commands.Users.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(request.Id, cancellationToken);

            if (existingUser == null)
                throw new UserNotFoundException();

            var userUpdated = new UserDto()
            {
                Username = request.Username,
                Email = request.Email,
            };

            await _userRepository.UpdateUserAsync(existingUser, userUpdated, cancellationToken);

            return existingUser.Username;
        }
    }
}
