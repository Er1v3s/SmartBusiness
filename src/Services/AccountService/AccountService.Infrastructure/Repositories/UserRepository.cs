using Microsoft.EntityFrameworkCore;
using AccountService.Application.Abstracts;
using AccountService.Domain.Entities;

namespace AccountService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AccountServiceDbContext _dbContext;

        public UserRepository(AccountServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public IQueryable<User> GetQueryableIncludingProperties()
        {
            return _dbContext.Users
                .Include(u => u.UserCompanyRoles)
                    .ThenInclude(ucr => ucr.Company)
                .Include(u => u.UserCompanyRoles)
                    .ThenInclude(ucr => ucr.Role)
                .AsQueryable();
        }

        public async Task<User?> GetFilteredUserAsync(IQueryable<User> query, CancellationToken cancellationToken)
        {
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> IsEmailInUseAsync(string email)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User updatedUser)
        {
            var existingUser = (await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == updatedUser.Id))!;
            _dbContext.Entry(existingUser).CurrentValues.SetValues(updatedUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeUserPasswordAsync(User user, string newPassword)
        {
            user.PasswordHash = newPassword;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
    }
}