using WriteService.Domain.Entities;

namespace WriteService.Application.Abstracts
{
    public interface ITransactionRepository
    {
        public Task AddAsync(Transaction transaction);
        public Task UpdateAsync(string id, Transaction transaction);
        public Task DeleteAsync(string id);
    }
}
