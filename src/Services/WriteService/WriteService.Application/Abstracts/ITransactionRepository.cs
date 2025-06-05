using WriteService.Domain.Entities;

namespace WriteService.Application.Abstracts
{
    public interface ITransactionRepository
    {
        public Task<List<Transaction>> GetAllAsync();
        public Task<Transaction?> GetByIdAsync(string id);
        public Task<List<Transaction>> GetByCompanyIdAsync(string companyId);
        public Task<List<Transaction>> GetByUserIdAsync(string userId);
        public Task AddAsync(Transaction transaction);
        public Task UpdateAsync(string id, Transaction transaction);
        public Task DeleteAsync(string id);
    }
}
