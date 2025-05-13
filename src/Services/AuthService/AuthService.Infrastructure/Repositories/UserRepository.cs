using AuthService.Application.Abstracts;
using AuthService.Contracts.DTOs;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthServiceDbContext _dbContext;

        public UserRepository(AuthServiceDbContext dbContext)
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

        //public async Task UpdateUserAsync(User existingUser, UserDto userUpdated, CancellationToken cancellationToken)
        //{
        //    if (existingUser.Email != userUpdated.Email)
        //        existingUser.Email = userUpdated.Email;

        //    if (existingUser.Username != userUpdated.Username)
        //        existingUser.Username = userUpdated.Username;

        //    existingUser.RefreshToken = userUpdated.RefreshToken;
        //    existingUser.RefreshTokenExpiresAtUtc = userUpdated.RefreshTokenExpiresAtUtc;

        //    _dbContext.Users.Update(existingUser);
        //    await _dbContext.SaveChangesAsync(cancellationToken);
        //}

        public async Task UpdateUserAsync(User updatedUser)
        {
            var existingUser = (await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == updatedUser.Id))!;
            _dbContext.Entry(existingUser).CurrentValues.SetValues(updatedUser);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangeUserPasswordAsync(User user, string newPassword, CancellationToken cancellationToken)
        {
            user.PasswordHash = newPassword;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
    }
}
