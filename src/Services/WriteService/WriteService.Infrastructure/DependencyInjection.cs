using Microsoft.Extensions.DependencyInjection;
using WriteService.Application.Abstracts;
using WriteService.Infrastructure.Repositories;

namespace WriteService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
