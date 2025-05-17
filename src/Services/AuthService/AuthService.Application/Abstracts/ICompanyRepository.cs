using AuthService.Domain.Entities;

namespace AuthService.Application.Abstracts
{
    public interface ICompanyRepository
    {
        Task AddCompanyAsync(Company company);
        Task UpdateCompanyAsync(Company company);
        Task DeleteCompanyAsync(Company company);
        Task<Company?> GetCompanyByIdAsync(string id);
        IQueryable<Company> GetQueryable();
        IQueryable<Company> GetQueryableIncludingProperties();
        Task<Company?> GetFilteredCompanyAsync(IQueryable<Company> query, CancellationToken cancellationToken);
        Task<List<Company>> GetFilteredCompaniesAsync(IQueryable<Company> query, CancellationToken cancellationToken);
        Task<bool> IsUserOwnerOfCompanyAsync(Guid userId, string companyId);
    }
}
