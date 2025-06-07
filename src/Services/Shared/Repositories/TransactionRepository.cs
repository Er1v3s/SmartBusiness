using MongoDB.Driver;
using Shared.Abstracts;
using Shared.Entities;

namespace Shared.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactionsCollection;

        public TransactionRepository(IMongoDatabase database)
        {
            _transactionsCollection = database.GetCollection<Transaction>("transactions");
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _transactionsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(string id)
        {
            return await _transactionsCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Transaction>> GetByCompanyIdAsync(string companyId)
        {
            return await _transactionsCollection.Find(t => t.CompanyId == companyId).ToListAsync();
        }

        public async Task<List<Transaction>> GetByUserIdAsync(string userId)
        {
            return await _transactionsCollection.Find(t => t.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _transactionsCollection.InsertOneAsync(transaction);
        }

        public async Task UpdateAsync(string id, Transaction transaction)
        {
            await _transactionsCollection.ReplaceOneAsync(t => t.Id == id, transaction);
        }

        public async Task DeleteAsync(string id)
        {
            await _transactionsCollection.DeleteOneAsync(t => t.Id == id);
        }
    }
}
