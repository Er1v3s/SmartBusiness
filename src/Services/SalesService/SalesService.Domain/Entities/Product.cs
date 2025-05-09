namespace SalesService.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Category { get; set; } = new();
        public decimal Price { get; set; }
        public int Tax { get; set; }
        public string? ImageFile { get; set; } = null;
        public DateTime CreatedAt { get; private set; } = DateTime.Now.ToUniversalTime();
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
