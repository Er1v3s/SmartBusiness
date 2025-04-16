using MediatR;

namespace SmartBusiness.Application.Commands.Authentication.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : IRequest<string> { }
}
