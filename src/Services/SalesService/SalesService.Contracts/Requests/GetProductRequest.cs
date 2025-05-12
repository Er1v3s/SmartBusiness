namespace SalesService.Contracts.Requests
{
    public record GetProductRequest(string? Id, string? Name, string? Category, decimal? MinPrice, decimal? MaxPrice);
}
