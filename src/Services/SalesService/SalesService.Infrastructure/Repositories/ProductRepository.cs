using Microsoft.EntityFrameworkCore;
using SalesService.Application.Abstracts;
using SalesService.Contracts.Dtos;
using SalesService.Domain.Entities;
using Shared.Exceptions;

namespace SalesService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SalesDbContext _dbContext;

        public ProductRepository(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProductAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product updatedProduct)
        {
            var productFromDb = (await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == updatedProduct.Id))!;
            _dbContext.Entry(productFromDb).CurrentValues.SetValues(updatedProduct);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Product> GetQueryable(CancellationToken cancellationToken)
        {
            return _dbContext.Products.AsQueryable();
        }

        public async Task<List<Product>> GetFilteredProductsAsync(IQueryable<Product> query, CancellationToken cancellationToken)
        {
            return await query.ToListAsync(cancellationToken);
        }
    }
}
