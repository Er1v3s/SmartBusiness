using MediatR;
using SmartBusiness.Application.Commands.Users;

namespace SmartBusiness.Application.Commands.Users.Create
{
    public record CreateUserCommand(string Username, string Email, string Password) 
        : UserCommand(Username, Email), IRequest<string> { }
}
