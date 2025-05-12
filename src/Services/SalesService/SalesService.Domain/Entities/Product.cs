using NanoidDotNet;

namespace SalesService.Domain.Entities
{
    public class Product
    {
        public string Id { get; private set; } = Nanoid.Generate(size: 17);
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Category { get; set; } = new();
        public decimal Price { get; set; } = 0;
        public int Tax { get; set; } = 0;
        public string? ImageFile { get; set; } = null;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
