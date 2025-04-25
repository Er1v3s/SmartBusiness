using SmartBusiness.Contracts.DataTransferObjects;
using SmartBusiness.Domain.Entities;

namespace SmartBusiness.Application.Abstracts
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> IsEmailInUseAsync(string email, CancellationToken cancellationToken);
        Task AddUserAsync(User user, CancellationToken cancellationToken);
        Task UpdateUserAsync(User existingUser, UserDto userUpdated, CancellationToken cancellationToken);
        Task DeleteUserAsync(User user, CancellationToken cancellationToken);
        Task ChangeUserPasswordAsync(User user, string newPassword, CancellationToken cancellationToken);
    }
}
