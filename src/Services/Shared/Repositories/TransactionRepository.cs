using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

        public async Task AddAsync(Transaction transaction)
        {
            await _transactionsCollection.InsertOneAsync(transaction);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await _transactionsCollection.ReplaceOneAsync(t => t.Id == transaction.Id, transaction);
        }

        public async Task DeleteAsync(string id)
        {
            await _transactionsCollection.DeleteOneAsync(t => t.Id == id);
        }

        public async Task DeleteManyAsync(string companyId)
        {
            await _transactionsCollection.DeleteManyAsync(t => t.CompanyId == companyId);
        }

        public async Task<Transaction?> GetTransactionByIdAsync(string id)
        {
            return await _transactionsCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public IQueryable<Transaction> GetQueryable()
        {
            return _transactionsCollection.AsQueryable();
        }

        public async Task<List<Transaction>> GetFilteredTransactionsAsync(IQueryable<Transaction> query, CancellationToken cancellation)
        {
            return await query.ToListAsync(cancellation);
        }
    }
}
