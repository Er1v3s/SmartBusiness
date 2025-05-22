using AuthService.Application.Abstracts;
using AuthService.Domain.DataTypes;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AuthServiceDbContext _dbContext;

        public CompanyRepository(AuthServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCompanyAsync(Company company)
        {
            await _dbContext.Companies.AddAsync(company);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCompanyAsync(Company updatedCompany)
        {
            var companyFromDb = await _dbContext.Companies.FirstAsync(c => c.Id == updatedCompany.Id);
            _dbContext.Entry(companyFromDb).CurrentValues.SetValues(updatedCompany);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCompanyAsync(Company company)
        {
            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Company> GetQueryable()
        {
            return _dbContext.Companies.AsQueryable();
        }

        public IQueryable<Company> GetQueryableIncludingProperties()
        {
            return _dbContext.Companies
                .Include(c => c.UserCompanyRoles)
                .ThenInclude(ucr => ucr.User)
                .Include(c => c.UserCompanyRoles)
                .ThenInclude(ucr => ucr.Role)
                .AsQueryable();
        }

        public async Task<Company?> GetFilteredCompanyAsync(IQueryable<Company> query, CancellationToken cancellationToken)
        {
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Company>> GetFilteredCompaniesAsync(IQueryable<Company> query, CancellationToken cancellationToken)
        {
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Company?> GetCompanyByIdAsync(string id)
        {
            return await _dbContext.Companies
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsUserOwnerOfCompanyAsync(Guid userId, string companyId)
        {
            return await _dbContext.UserCompanyRoles
                .AnyAsync(ucr => ucr.UserId == userId && ucr.CompanyId == companyId && ucr.Role.Name == RoleType.Owner);
        }
    }
}