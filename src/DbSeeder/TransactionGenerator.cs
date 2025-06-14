using SalesService.Domain.Entities;
using Shared.Entities;

namespace DbSeeder
{
    public class TransactionGenerator
    {
        private readonly string _companyId;
        private readonly string _userId;
        private readonly List<Service> _services;
        private readonly Random _rand = new();

        public TransactionGenerator(List<Service> services, string companyId, string userId)
        {
            _services = services;
            _companyId = companyId;
            _userId = userId;
        }

        public List<Transaction> GenerateTransactions(DateTime startDate, DateTime endDate)
        {
            var transactions = new List<Transaction>();

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                var schedule = GenerateDaySchedule(date);
                transactions.AddRange(schedule);
            }

            return transactions;
        }

        private List<Transaction> GenerateDaySchedule(DateTime day)
        {
            var transactions = new List<Transaction>();

            var startTime = day.AddHours(8); // 8:00
            var endTime = day.AddHours(19);  // max koniec

            // 🔁 Określ sezonowość
            double seasonalMultiplier = GetSeasonalMultiplier(day.Month);
            int baseAppointmentsPerDay = 6; // Załóżmy, że w najlepszym okresie średnio 6 usług dziennie
            int appointmentsToday = (int)Math.Round(baseAppointmentsPerDay * seasonalMultiplier);

            var currentTime = startTime;

            for (int i = 0; i < appointmentsToday; i++)
            {
                // 20% szans na przerwę przed usługą
                if (_rand.NextDouble() < 0.2)
                {
                    currentTime = currentTime.AddMinutes(_rand.Next(60, 121));
                }

                var service = PickService(currentTime);
                var serviceEnd = currentTime.AddMinutes((double)service.Duration!);
                if (serviceEnd > endTime)
                    break;

                var transaction = new Transaction
                {
                    CompanyId = _companyId,
                    UserId = _userId,
                    ItemId = service.Id,
                    ItemType = "service",
                    Quantity = 1,
                    TotalAmount = service.Price,
                    Tax = service.Tax,
                    CreatedAt = currentTime
                };

                transactions.Add(transaction);

                currentTime = serviceEnd.AddMinutes(_rand.Next(5, 16));
            }

            return transactions;
        }

        private double GetSeasonalMultiplier(int month)
        {
            switch (month)
            {
                case 1:
                    return 0.50;
                case 2:
                    return 0.55;
                case 3:
                    return 0.70;
                case 4:
                    return 0.75;
                case 5:
                    return 1.1;
                case 6:
                    return 1.0; 
                case 7:
                    return 1.1;
                case 8:
                    return 1.1;
                case 9:
                    return 0.95;
                case 10:
                    return 0.75;
                case 11:
                    return 0.55;
                case 12:
                    return 0.8;
                default:
                    return 0.75;
            }
        }

        private Service PickService(DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            var day = date.Day;
            var month = date.Month;

            // 🌸 SEZON MAKIJAŻY: maj–październik
            bool isMakeupSeason = month >= 5 && month <= 10;
            double makeupBoostChance = isMakeupSeason ? 0.3 : 0.1; // 3x większa szansa w sezonie

            // 💰 WYPŁATA: 10–17 => +200%, 18–24 => +100%
            double expensiveBoostChance = 0.05; // bazowa szansa na drogą usługę
            if (day >= 10 && day <= 17)
                expensiveBoostChance *= 3;
            else if (day >= 18 && day <= 24)
                expensiveBoostChance *= 2;

            // Czy preferować makijaż?
            if (_rand.NextDouble() < makeupBoostChance)
            {
                var makeupServices = _services.Where(s => s.Category == "Makijaż").ToList();
                if (makeupServices.Any())
                {
                    return PickServiceByPriceBoost(makeupServices, expensiveBoostChance);
                }
            }

            return PickServiceByPriceBoost(_services, expensiveBoostChance);
        }

        private Service PickServiceByPriceBoost(List<Service> pool, double expensiveChance)
        {
            // Zwiększone szanse na wybór drogiej usługi
            if (_rand.NextDouble() < expensiveChance)
            {
                var expensive = pool.Where(s => s.Price >= 500).ToList();
                if (expensive.Any())
                    return expensive[_rand.Next(expensive.Count)];
            }

            return pool[_rand.Next(pool.Count)];
        }
    }
}
