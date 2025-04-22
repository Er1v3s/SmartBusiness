using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartBusiness.Application.Abstracts;
using SmartBusiness.Contracts.DataTransferObjects;
using SmartBusiness.Contracts.Errors;
using SmartBusiness.Domain.Entities;

namespace SmartBusiness.Application.Commands.Users.Update
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
                throw new NotFoundException("User not found");

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
