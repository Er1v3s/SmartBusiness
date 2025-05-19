using AuthService.Application.Abstracts;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class UserCompanyRoleRepository : IUserCompanyRoleRepository
    {
        private readonly AuthServiceDbContext _dbContext;

        public UserCompanyRoleRepository(AuthServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserCompanyRoleAsync(UserCompanyRole ucr)
        {
            await _dbContext.UserCompanyRoles.AddAsync(ucr);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserCompanyRoleAsync(UserCompanyRole ucr)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveUserCompanyRoleAsync(UserCompanyRole ucr)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserCompanyRole> GetQueryable()
        {
            return _dbContext.UserCompanyRoles.AsQueryable();
        }

        public IQueryable<UserCompanyRole> GetQueryableIncludingProperties()
        {
            return _dbContext.UserCompanyRoles
                .Include(u => u.User)
                .Include(u => u.Company)
                .AsQueryable();
        }

        public async Task<UserCompanyRole?> GetFilteredUserCompanyRoleAsync(IQueryable<UserCompanyRole> query, CancellationToken cancellationToken)
        {
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<UserCompanyRole>> GetFilteredUserCompanyRolesAsync(IQueryable<UserCompanyRole> query, CancellationToken cancellationToken)
        {
            return await query.ToListAsync(cancellationToken);
        }
    }
}