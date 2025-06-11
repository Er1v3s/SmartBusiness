using Shared.Entities;

namespace Shared.Abstracts
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(string id);
        Task DeleteManyAsync(string companyId);
        Task<Transaction?> GetTransactionByIdAsync(string id);
        IQueryable<Transaction> GetQueryable();
        Task<List<Transaction>> GetFilteredTransactionsAsync(IQueryable<Transaction> query, CancellationToken cancellation);
    }
}
