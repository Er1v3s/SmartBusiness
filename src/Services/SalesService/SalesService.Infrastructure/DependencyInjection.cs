using Microsoft.Extensions.DependencyInjection;
using SalesService.Application.Abstracts;
using SalesService.Infrastructure.Repositories;

namespace SalesService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
