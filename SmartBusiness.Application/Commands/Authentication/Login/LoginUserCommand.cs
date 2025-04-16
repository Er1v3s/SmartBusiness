using MediatR;

namespace SmartBusiness.Application.Commands.Authentication.Login
{
    public record LoginUserCommand(string Email, string Password) : IRequest<string> { }
}
