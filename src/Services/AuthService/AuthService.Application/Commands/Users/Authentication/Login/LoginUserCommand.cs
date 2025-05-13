using MediatR;

namespace AuthService.Application.Commands.Users.Authentication.Login
{   
    public record LoginUserCommand(string Email, string Password) : IRequest { }
}
