namespace SalesService.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
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
