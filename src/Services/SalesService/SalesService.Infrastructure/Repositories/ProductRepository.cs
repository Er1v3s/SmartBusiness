using Microsoft.EntityFrameworkCore;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;

namespace SalesService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SalesServiceDbContext _dbContext;

        public ProductRepository(SalesServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProductAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var productToUpdate = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            _dbContext.Entry(productToUpdate).CurrentValues.SetValues(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Product?> GetProductByIdAsync(string id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Product> GetQueryable()
        {
            return _dbContext.Products.AsQueryable();
        }

        public async Task<List<Product>> GetFilteredProductsAsync(IQueryable<Product> query, CancellationToken cancellationToken)
        {
            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
