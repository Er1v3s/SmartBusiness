using MediatR;

namespace AuthService.Application.Commands.Users.ChangePassword
{
    public record ChangeUserPasswordCommand(Guid Id, string CurrentPassword, string NewPassword) : IRequest<string> {}
}
