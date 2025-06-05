using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WriteService.Application.Abstracts;
using WriteService.Domain.Entities;

namespace WriteService.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactions;

        public TransactionRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _transactions = database.GetCollection<Transaction>("transactions");
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _transactions.Find(_ => true).ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(string id)
        {
            return await _transactions.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Transaction>> GetByCompanyIdAsync(string companyId)
        {
            return await _transactions.Find(t => t.CompanyId == companyId).ToListAsync();
        }

        public async Task<List<Transaction>> GetByUserIdAsync(string userId)
        {
            return await _transactions.Find(t => t.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _transactions.InsertOneAsync(transaction);
        }

        public async Task UpdateAsync(string id, Transaction transaction)
        {
            await _transactions.ReplaceOneAsync(t => t.Id == id, transaction);
        }

        public async Task DeleteAsync(string id)
        {
            await _transactions.DeleteOneAsync(t => t.Id == id);
        }
    }
}
