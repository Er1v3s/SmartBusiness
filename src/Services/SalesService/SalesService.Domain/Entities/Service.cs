using NanoidDotNet;

namespace SalesService.Domain.Entities
{
    public class Service
    {
        public string Id { get; private set; } = Nanoid.Generate(size: 15);
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public int Tax { get; set; } = 0;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public int? Duration { get; set; } = null;
    }
}
