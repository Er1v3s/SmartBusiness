using Shared.Entities;

namespace Shared.Abstracts
{
    public interface ITransactionRepository
    {
        //Task<List<Transaction>> GetAllAsync();
        //Task<List<Transaction>> GetByCompanyIdAsync(string companyId);
        //Task<List<Transaction>> GetByUserIdAsync(string userId);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(string id);
        Task<Transaction?> GetTransactionByIdAsync(string id);
        IQueryable<Transaction> GetQueryable();
        Task<List<Transaction>> GetFilteredTransactionsAsync(IQueryable<Transaction> query, CancellationToken cancellation);
    }
}
