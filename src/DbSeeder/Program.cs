using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Shared.Entities;
using SalesService.Domain.Entities;
using SalesService.Infrastructure;

namespace DbSeeder
{
    class Program
    {
        const string CompanyId = "a-SHEM7qn4hM_90D6";
        const string UserId = "682619a2-1fe7-4124-9676-f8eb50928b12";
        const bool UseProducts = true;
        const bool UseServices = false;
        static readonly DateTime StartDate = new (2020, 1, 1);
        static readonly DateTime EndDate = new(2021, 12, 31);

        static void Main()
        {
            var options = new DbContextOptionsBuilder<SalesServiceDbContext>()
                .UseSqlServer("Server=localhost,1433;Database=SmartBusinessSalesDb;User Id=SA;Password=SuperPassword123;TrustServerCertificate=True;");
            var dbContext = new SalesServiceDbContext(options.Options);

            var client1 = new MongoClient("mongodb://SA:SuperPassword123@localhost:27017/SmartBusinessWriteDb?authSource=admin");
            var database1 = client1.GetDatabase("SmartBusinessWriteDb");
            var transactionsCollection1 = database1.GetCollection<Transaction>("transactions");

            var client2 = new MongoClient("mongodb://SA:SuperPassword123@localhost:27017/SmartBusinessReadDb?authSource=admin");
            var database2 = client2.GetDatabase("SmartBusinessReadDb");
            var transactionsCollection2 = database2.GetCollection<Transaction>("transactions");

            

            //var products = GenerateProducts();
            //var services = ProductsAndServices.GenerateServices(CompanyId);

            // COMMENT THIS CODE IF YOU ARE ADDING MORE TRANSACTIONS
            //dbContext.Services.AddRange(services);
            //dbContext.Products.AddRange(products);
            //dbContext.SaveChanges();
            // COMMENT THIS CODE IF YOU ARE ADDING MORE TRANSACTIONS

            //var productsWithId = dbContext.Products.Where(p => p.CompanyId == CompanyId).ToList();
            var servicesWithId = dbContext.Services.Where(p => p.CompanyId == CompanyId).ToList();


            var generator = new TransactionGenerator(servicesWithId, CompanyId, UserId);
            var transactions = generator.GenerateTransactions(StartDate, EndDate);


            // Insert do MongoDB
            transactionsCollection1.InsertMany(transactions);
            transactionsCollection2.InsertMany(transactions);
            Console.WriteLine($"Wygenerowano i zapisano {transactions.Count} transakcji.");
        }
    }
}
