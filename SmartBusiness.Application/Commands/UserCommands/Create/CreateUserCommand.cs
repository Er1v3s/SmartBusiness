using MediatR;

namespace SmartBusiness.Application.Commands.UserCommands.Create
{
    public record CreateUserCommand(string Username, string Email, string Password) 
        : UserCommand(Username, Email), IRequest<string> { }
}
