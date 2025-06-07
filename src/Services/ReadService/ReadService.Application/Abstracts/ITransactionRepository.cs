using ReadService.Domain.Entities;

namespace ReadService.Application.Abstracts
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(string id);
        Task<List<Transaction>> GetByCompanyIdAsync(string companyId);
        Task<List<Transaction>> GetByUserIdAsync(string userId);
    }
}
