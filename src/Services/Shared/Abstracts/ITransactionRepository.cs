using Shared.Entities;

namespace Shared.Abstracts
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(string id);
        Task<List<Transaction>> GetByCompanyIdAsync(string companyId);
        Task<List<Transaction>> GetByUserIdAsync(string userId);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(string id, Transaction transaction);
        Task DeleteAsync(string id);
    }
}
