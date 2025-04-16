using MediatR;

namespace SmartBusiness.Application.Commands.UserCommands.Update
{
    public record UpdateUserCommand(Guid Id, string Username, string Email) 
        : UserCommand(Username, Email), IRequest<string> { }
}
