﻿using SalesService.Domain.Entities;

namespace SalesService.Application.Abstracts
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task DeleteManyProductsByCompanyIdAsync(string companyId);
        Task<Product?> GetProductByIdAsync(string productId);
        IQueryable<Product> GetQueryable();
        Task<List<Product>> GetFilteredProductsAsync(IQueryable<Product> query, CancellationToken cancellationToken);
    }
}
