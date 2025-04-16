using MediatR;

namespace SmartBusiness.Application.Commands.UserCommands.ChangePassword
{
    public record ChangeUserPasswordCommand(Guid Id, string CurrentPassword, string NewPassword) : IRequest<string> {}
}
