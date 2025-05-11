namespace SalesService.Contracts.Requests
{
    public record CreateProductRequest(string Name, string Description, List<string> Category, decimal Price,
        int Tax, string ImageFile);
}
