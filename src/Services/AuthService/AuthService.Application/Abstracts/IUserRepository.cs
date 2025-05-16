using AuthService.Domain.Entities;

namespace AuthService.Application.Abstracts
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        IQueryable<User> GetQueryableIncludingProperties();
        Task<User?> GetFilteredUserAsync(IQueryable<User> query, CancellationToken cancellationToken);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<bool> IsEmailInUseAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User updatedUser);
        Task DeleteUserAsync(User user);
        Task ChangeUserPasswordAsync(User user, string newPassword);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
