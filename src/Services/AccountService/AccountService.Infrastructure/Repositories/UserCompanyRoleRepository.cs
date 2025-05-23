using Microsoft.EntityFrameworkCore;
using AccountService.Application.Abstracts;
using AccountService.Domain.Entities;

namespace AccountService.Infrastructure.Repositories
{
    public class UserCompanyRoleRepository : IUserCompanyRoleRepository
    {
        private readonly AccountServiceDbContext _dbContext;

        public UserCompanyRoleRepository(AccountServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserCompanyRoleAsync(UserCompanyRole ucr)
        {
            await _dbContext.UserCompanyRoles.AddAsync(ucr);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserCompanyRoleAsync(UserCompanyRole updatedUcr)
        {
            var ucrFromDb = await _dbContext.UserCompanyRoles.FirstAsync(ucr => ucr.RoleId == updatedUcr.RoleId);
            _dbContext.Entry(ucrFromDb).CurrentValues.SetValues(updatedUcr);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveUserCompanyRoleAsync(UserCompanyRole ucr)
        {
            _dbContext.UserCompanyRoles.Remove(ucr);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<UserCompanyRole> GetQueryable()
        {
            return _dbContext.UserCompanyRoles.AsQueryable();
        }

        public IQueryable<UserCompanyRole> GetQueryableIncludingProperties()
        {
            return _dbContext.UserCompanyRoles
                .Include(ucr => ucr.User)
                .Include(ucr => ucr.Company)
                .Include(ucr => ucr.Role)
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