using Microsoft.Extensions.DependencyInjection;
using Shared.Abstracts;
using Shared.Repositories;

namespace ReadService.Infrastructure
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
