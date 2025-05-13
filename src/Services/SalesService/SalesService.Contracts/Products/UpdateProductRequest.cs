namespace SalesService.Contracts.Products
{
    public record UpdateProductRequest(string Name, string Description, List<string> Category, decimal Price,
        int Tax, string ImageFile);
}