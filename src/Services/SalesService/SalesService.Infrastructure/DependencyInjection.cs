using Microsoft.Extensions.DependencyInjection;
using SalesService.Application.Abstracts;
using SalesService.Infrastructure.Repositories;
using Shared.Abstracts;
using Shared.MessageBroker;

namespace SalesService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IEventBus, EventBus>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();

            return services;
        }
    }
}
