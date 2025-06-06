using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReadService.Application.Abstracts;
using ReadService.Domain.Entities;

namespace ReadService.Infrastructure.Repositories
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
    }
}
