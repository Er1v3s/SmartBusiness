using MediatR;
using SmartBusiness.Domain.Entities;

namespace SmartBusiness.Application.Commands.Auth.RegisterUser
{
    public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<string> { }
}
