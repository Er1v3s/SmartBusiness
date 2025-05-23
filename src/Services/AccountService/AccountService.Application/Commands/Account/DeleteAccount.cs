using MediatR;
using AccountService.Application.Abstracts;
using AccountService.Contracts.Exceptions.Users;

namespace AccountService.Application.Commands.Account
{
    public record DeleteUserCommand(Guid Id) : IRequest;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id)
                ?? throw new UserNotFoundException();

            await _userRepository.DeleteUserAsync(user);
        }
    }
}
