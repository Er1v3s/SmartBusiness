using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SmartBusiness.Application.Abstracts;
using SmartBusiness.Contracts.DataTransferObjects;
using SmartBusiness.Domain.Entities;

namespace SmartBusiness.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SmartBusinessDbContext _dbContext;

        public UserRepository(SmartBusinessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<bool> IsEmailInUseAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserAsync(User existingUser, UserDto userUpdated, CancellationToken cancellationToken)
        {
            if (existingUser.Email != userUpdated.Email)
                existingUser.Email = userUpdated.Email;

            if (existingUser.Username != userUpdated.Username)
                existingUser.Username = userUpdated.Username;

            _dbContext.Users.Update(existingUser);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            await _dbContext.Users
                .Where(u => u.Id == userId)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task ChangeUserPasswordAsync(User user, string newPassword, CancellationToken cancellationToken)
        {
            user.PasswordHash = newPassword;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
