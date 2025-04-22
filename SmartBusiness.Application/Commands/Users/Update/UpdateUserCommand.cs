using MediatR;

namespace SmartBusiness.Application.Commands.Users.Update
{
    public record UpdateUserCommand(Guid Id, string Username, string Email) 
        : UserCommand(Username, Email), IRequest<string> { }
}
