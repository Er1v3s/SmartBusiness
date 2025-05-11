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

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null)
                throw new NotFoundException($"Product with id: {id} was not found");
            
            return product;
        }

        public async Task<List<Product>> GetProductsByNameAsync(string name)
        {
            return await _dbContext.Products
                .Where(p => p.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken)
        {
            return await _dbContext.Products
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken)
        {
            return await _dbContext.Products
                .Where(p => p.Category.Contains(category))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Products.ToListAsync(cancellationToken);
        }

        public async Task AddProductAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            var productFromDb = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productDto.Id);
            if(productFromDb == null)
                throw new NotFoundException("Product not found");
            
            productFromDb.Name = productDto.Name;
            productFromDb.Description = productDto.Description;
            productFromDb.Category = productDto.Category;
            productFromDb.Price = productDto.Price;
            productFromDb.Tax = productDto.Tax;
            productFromDb.ImageFile = productDto.ImageFile;
            productFromDb.UpdatedAt = productDto.UpdatedAt;
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await GetProductByIdAsync(id);
            if (product == null)
                throw new NotFoundException("Product not found");
            
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ProductExistsAsync(Guid id)
        {
            return await _dbContext.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbContext.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();
        }
    }
}
