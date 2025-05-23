using MediatR;
using Microsoft.AspNetCore.Identity;
using AccountService.Domain.Entities;
using AccountService.Application.Abstracts;
using AccountService.Contracts.Exceptions.Users;
using AccountService.Application.Commands.Abstracts;

namespace AccountService.Application.Commands.Account
{
    public record UpdateUserCommand(Guid Id, string? Username, string? Email)
        : UserCommand(Username, Email), IRequest;

    public class UpdateUserCommandValidator : UserCommandValidator<UpdateUserCommand>;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id)
                ?? throw new UserNotFoundException();

            if(!string.IsNullOrEmpty(request.Username))
                user.Username = request.Username;

            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email;

            await _userRepository.UpdateUserAsync(user);
        }
    }
}
