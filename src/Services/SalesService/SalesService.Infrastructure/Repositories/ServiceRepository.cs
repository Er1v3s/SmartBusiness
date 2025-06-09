using Microsoft.EntityFrameworkCore;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;

namespace SalesService.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly SalesServiceDbContext _dbContext;

        public ServiceRepository(SalesServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddServiceAsync(Service service)
        {
            await _dbContext.Services.AddAsync(service);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateServiceAsync(Service service)
        {
            var serviceToUpdate = await _dbContext.Services
                .FirstOrDefaultAsync(s => s.Id == service.Id);

            _dbContext.Entry(serviceToUpdate).CurrentValues.SetValues(service);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteServiceAsync(Service service)
        {
            _dbContext.Services.Remove(service);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Service?> GetServiceByIdAsync(string id)
        {
            return await _dbContext.Services.FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Service> GetQueryable()
        {
            return _dbContext.Services.AsQueryable();
        }

        public async Task<List<Service>> GetFilteredServicesAsync(IQueryable<Service> query, CancellationToken cancellationToken)
        {
            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
