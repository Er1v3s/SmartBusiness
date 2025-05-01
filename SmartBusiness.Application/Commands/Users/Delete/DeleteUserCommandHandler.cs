using MediatR;
using AuthService.Application.Abstracts;
using AuthService.Contracts.Exceptions.Users;

namespace AuthService.Application.Commands.Users.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id, cancellationToken);

            if (user == null)
                throw new UserNotFoundException();

            await _userRepository.DeleteUserAsync(user, cancellationToken);
        }
    }
}
