using MediatR;

namespace SmartBusiness.Application.Commands.Authentication.ChangeUserPassword
{
    public record ChangeUserPasswordCommand(Guid Id, string CurrentPassword, string NewPassword) : IRequest<string> {}
}
