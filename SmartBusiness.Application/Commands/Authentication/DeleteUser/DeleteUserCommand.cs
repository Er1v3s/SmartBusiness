using MediatR;

namespace SmartBusiness.Application.Commands.Authentication.DeleteUser
{
    public record DeleteUserCommand(Guid Id) : IRequest;
}
