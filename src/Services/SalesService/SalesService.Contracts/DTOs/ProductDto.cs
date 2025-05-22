namespace SalesService.Contracts.DTOs
{
    public class ProductDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required List<string> Category { get; set; }
        public required decimal Price { get; set; }
        public required int Tax { get; set; }
        public string? ImageFile { get; set; } = null;
        public required DateTime UpdatedAt { get; set; }
    }
}
