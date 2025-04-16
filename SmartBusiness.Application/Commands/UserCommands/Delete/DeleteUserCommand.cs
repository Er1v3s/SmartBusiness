using MediatR;

namespace SmartBusiness.Application.Commands.UserCommands.Delete
{
    public record DeleteUserCommand(Guid Id) : IRequest;
}
