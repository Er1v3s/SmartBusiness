using MediatR;

namespace SmartBusiness.Application.Commands.Authentication.UpdateUser
{
    public record UpdateUserCommand(Guid Id, string Username, string Email) 
        : UserCommand(Username, Email), IRequest<string> { }
}
