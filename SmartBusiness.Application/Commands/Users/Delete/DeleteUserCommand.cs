using MediatR;

namespace SmartBusiness.Application.Commands.Users.Delete
{
    public record DeleteUserCommand(Guid Id) : IRequest;
}
