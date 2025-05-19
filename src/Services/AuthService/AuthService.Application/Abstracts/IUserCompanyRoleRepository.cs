using AuthService.Domain.Entities;

namespace AuthService.Application.Abstracts
{
    public interface IUserCompanyRoleRepository
    {
        Task AddUserCompanyRoleAsync(UserCompanyRole ucr);
        Task UpdateUserCompanyRoleAsync(UserCompanyRole ucr);
        Task RemoveUserCompanyRoleAsync(UserCompanyRole ucr);
        IQueryable<UserCompanyRole> GetQueryable();
        IQueryable<UserCompanyRole> GetQueryableIncludingProperties();
        Task<UserCompanyRole?> GetFilteredUserCompanyRoleAsync(IQueryable<UserCompanyRole> query, CancellationToken cancellationToken);
        Task<List<UserCompanyRole>> GetFilteredUserCompanyRolesAsync(IQueryable<UserCompanyRole> query,
            CancellationToken cancellationToken);

    }
}