using MediatR;
using SmartBusiness.Application.Abstracts;
using SmartBusiness.Contracts.Errors;

namespace SmartBusiness.Application.Commands.Users.Delete
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
                throw new NotFoundException("User not found");

            await _userRepository.DeleteUserAsync(request.Id, cancellationToken);
        }
    }
}
