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
        const int NumberOfTransactions = 1000;
        static readonly DateTime StartDate = new (2023, 1, 1);
        static readonly DateTime EndDate = DateTime.Now;

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

            var random = new Random();

            var products = GenerateProducts();
            var services = GenerateServices();


            // COMMENT THIS CODE IF YOU ARE ADDING MORE TRANSACTIONS
            //dbContext.Services.AddRange(services);
            //dbContext.Products.AddRange(products);
            //dbContext.SaveChanges();
            // COMMENT THIS CODE IF YOU ARE ADDING MORE TRANSACTIONS

            var productsWithId = dbContext.Products.Where(p => p.CompanyId == CompanyId).ToList();
            var servicesWithId = dbContext.Services.Where(p => p.CompanyId == CompanyId).ToList();


            var transactions = new List<Transaction>();

            for (int i = 0; i < NumberOfTransactions; i++)
            {
                Transaction transaction;

                if (UseProducts && (!UseServices || random.NextDouble() < 0.5))
                {
                    var product = productsWithId[random.Next(productsWithId.Count)];
                    transaction = CreateTransactionFromProduct(product, random, StartDate, EndDate);
                }
                else
                {
                    var service = servicesWithId[random.Next(servicesWithId.Count)];
                    transaction = CreateTransactionFromService(service, random, StartDate, EndDate);
                }

                transactions.Add(transaction);
                
                Console.WriteLine($"Transakcja nr {i} wygenerowana: {transaction}");
            }

            // Insert do MongoDB
            transactionsCollection1.InsertMany(transactions);
            transactionsCollection2.InsertMany(transactions);
            Console.WriteLine($"Wygenerowano i zapisano {transactions.Count} transakcji.");
        }

        static Transaction CreateTransactionFromProduct(Product product, Random random, DateTime StartDate, DateTime EndDate)
        {
            var createdAt = RandomDate(random, StartDate, EndDate);
            var quantity = random.Next(1, 5);
            var total = quantity * product.Price;
            var tax = product.Tax;

            return new Transaction
            {
                CompanyId = CompanyId,
                UserId = UserId,
                ItemId = product.Id,
                ItemType = "product",
                Quantity = quantity,
                TotalAmount = total,
                Tax = tax,
                CreatedAt = createdAt,
            };
        }

        static Transaction CreateTransactionFromService(Service service, Random random, DateTime StartDate, DateTime EndDate)
        {
            var createdAt = RandomDate(random, StartDate, EndDate);
            var total = service.Price;
            var tax = service.Tax;

            return new Transaction
            {
                CompanyId = CompanyId,
                UserId = UserId,
                ItemId = service.Id,
                ItemType = "service",
                Quantity = 1,
                TotalAmount = total,
                Tax = tax,
                CreatedAt = createdAt,
            };
        }

        static DateTime RandomDate(Random random, DateTime start, DateTime end)
        {
            var range = (end - start).Days;
            return start.AddDays(random.Next(range)).AddHours(random.Next(0, 24)).AddMinutes(random.Next(0, 60));
        }

        static List<Product> GenerateProducts() => new()
        {
            new Product { CompanyId = CompanyId, Name = "Lakier hybrydowy", Description = "Trwały lakier do paznokci", Category = "Paznokcie", Price = 40, Tax = 23},
            new Product { CompanyId = CompanyId, Name = "Pilnik do paznokci", Description = "Profesjonalny pilnik do stylizacji", Category = "Paznokcie", Price = 15, Tax = 23 },
            new Product { CompanyId = CompanyId, Name = "Odżywka do paznokci", Description = "Wzmacniająca odżywka do paznokci", Category = "Paznokcie", Price = 50, Tax = 23 },
            new Product { CompanyId = CompanyId, Name = "Serum do twarzy", Description = "Intensywne serum nawilżające", Category = "Twarz", Price = 120, Tax = 23 },
            new Product { CompanyId = CompanyId, Name = "Krem pod oczy", Description = "Redukuje cienie i obrzęki", Category = "Twarz", Price = 90, Tax = 23 },
            new Product { CompanyId = CompanyId, Name = "Maseczka nawilżająca", Description = "Głęboko nawilżająca maseczka", Category = "Twarz", Price = 80, Tax = 23 },
            new Product { CompanyId = CompanyId, Name = "Pędzel do makijażu", Description = "Profesjonalny pędzel do aplikacji", Category = "Makijaż", Price = 60, Tax = 23 },
            new Product { CompanyId = CompanyId, Name = "Podkład do twarzy", Description = "Kryjący podkład o lekkiej formule", Category = "Makijaż", Price = 140, Tax = 23 },
        };

        static List<Service> GenerateServices() => new()
        {
            new Service { CompanyId = CompanyId, Name = "Manicure hybrydowy", Description = "Stylizacja paznokci lakierem hybrydowym", Category = "Paznokcie", Duration = 120, Price = 120, Tax = 8 },
            new Service { CompanyId = CompanyId, Name = "Pedicure klasyczny", Description = "Zabieg pielęgnacyjny dla stóp", Category = "Paznokcie", Duration = 60, Price = 150, Tax = 8 },
            new Service { CompanyId = CompanyId, Name = "Masaż twarzy", Description = "Relaksujący masaż poprawiający krążenie", Category = "Twarz", Duration = 60, Price = 90, Tax = 5 },
            new Service { CompanyId = CompanyId, Name = "Zabieg oczyszczający", Description = "Głębokie oczyszczanie skóry twarzy", Category = "Twarz", Duration = 60, Price = 200, Tax = 10 },
            new Service { CompanyId = CompanyId, Name = "Henna brwi i rzęs", Description = "Podkreślenie koloru brwi i rzęs", Category = "Brwi i rzęsy", Duration = 30, Price = 60, Tax = 5 },
            new Service { CompanyId = CompanyId, Name = "Laminacja brwi", Description = "Zabieg nadający brwiom idealny kształt", Category = "Brwi i rzęsy", Duration = 90, Price = 130, Tax = 8 },
            new Service { CompanyId = CompanyId, Name = "Makijaż okolicznościowy", Description = "Profesjonalny makijaż na specjalne okazje", Category = "Makijaż", Duration = 90, Price = 250, Tax = 12 },
        };
    }
}
