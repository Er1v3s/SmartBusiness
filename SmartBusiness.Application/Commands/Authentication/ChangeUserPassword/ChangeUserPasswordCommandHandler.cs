using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartBusiness.Contracts.Errors;
using SmartBusiness.Domain.Entities;
using SmartBusiness.Infrastructure;

namespace SmartBusiness.Application.Commands.Authentication.ChangeUserPassword
{
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, string>
    {
        private readonly SmartBusinessDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ChangeUserPasswordCommandHandler(SmartBusinessDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }


        public async Task<string> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
                throw new NotFoundException("User not found");

            if(_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword) != PasswordVerificationResult.Success)
                throw new InvalidPasswordException("Incorrect current password");

            var newPasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.NewPassword) == PasswordVerificationResult.Success)
                throw new ConflictException("New password cannot be the same as the old password");

            user.PasswordHash = newPasswordHash;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user.Username;
        }
    }
}
