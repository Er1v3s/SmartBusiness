using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartBusiness.Domain.Entities;
using SmartBusiness.Infrastructure;

namespace SmartBusiness.Application.Commands.Auth.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly SmartBusinessDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserCommandHandler(SmartBusinessDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if(existingUser != null)
                throw new Exception("User with this email already exists");

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
