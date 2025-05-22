using SalesService.Contracts.DTOs;
using SalesService.Domain.Entities;

namespace SalesService.Application.Abstracts
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(ProductDto product);
        Task DeleteProductAsync(ProductDto product);
        IQueryable<Product> GetQueryable();
        Task<List<Product>> GetFilteredProductsAsync(IQueryable<Product> query, CancellationToken cancellationToken);
    }
}
