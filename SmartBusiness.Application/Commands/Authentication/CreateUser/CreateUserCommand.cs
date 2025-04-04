using MediatR;

namespace SmartBusiness.Application.Commands.Authentication.CreateUser
{
    public record CreateUserCommand(string Username, string Email, string Password) 
        : UserCommand(Username, Email), IRequest<string> { }
}
