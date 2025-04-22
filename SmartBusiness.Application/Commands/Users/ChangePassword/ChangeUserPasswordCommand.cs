using MediatR;

namespace SmartBusiness.Application.Commands.Users.ChangePassword
{
    public record ChangeUserPasswordCommand(Guid Id, string CurrentPassword, string NewPassword) : IRequest<string> {}
}
