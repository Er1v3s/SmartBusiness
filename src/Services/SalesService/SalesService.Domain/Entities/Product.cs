using NanoidDotNet;

namespace SalesService.Domain.Entities
{
    public class Product
    {
        public string Id { get; private set; } = Nanoid.Generate(size: 21);
        public string CompanyId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public int Tax { get; set; } = 0;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
