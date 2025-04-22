using MediatR;

namespace SmartBusiness.Application.Commands.Users.Authentication.Login
{
    public record LoginUserCommand(string Email, string Password) : IRequest<string> { }
}
