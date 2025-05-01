using MediatR;

namespace AuthService.Application.Commands.Users.Delete
{
    public record DeleteUserCommand(Guid Id) : IRequest;
}
