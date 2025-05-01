using MediatR;

namespace AuthService.Application.Commands.Users.Update
{
    public record UpdateUserCommand(Guid Id, string Username, string Email) 
        : UserCommand(Username, Email), IRequest<string> { }
}
