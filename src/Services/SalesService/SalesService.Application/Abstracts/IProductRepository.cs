using SalesService.Domain.Entities;

namespace SalesService.Application.Abstracts
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(Guid id);
        Task<List<Product>> GetProductsByNameAsync(string name);
        Task<List<Product>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken);
        Task<List<Product>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken);
        Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
        Task<bool> ProductExistsAsync(Guid id);
        Task<List<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

    }
}
