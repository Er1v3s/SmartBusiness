using MediatR;

namespace AuthService.Application.Commands.Users.Create
{
    public record CreateUserCommand(string Username, string Email, string Password) 
        : UserCommand(Username, Email), IRequest<string> { }
}
