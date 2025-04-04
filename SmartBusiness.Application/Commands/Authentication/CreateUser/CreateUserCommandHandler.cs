using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartBusiness.Domain.Entities;
using SmartBusiness.Infrastructure;

namespace SmartBusiness.Application.Commands.Authentication.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly SmartBusinessDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public CreateUserCommandHandler(SmartBusinessDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _passwordHasher.HashPassword(new User(), request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash
            };

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user.Username;
        }
    }
}
