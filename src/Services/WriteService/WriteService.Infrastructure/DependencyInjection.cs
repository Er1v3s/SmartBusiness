using Microsoft.Extensions.DependencyInjection;
using Shared.Abstracts;
using Shared.MessageBroker;
using Shared.Repositories;

namespace WriteService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IEventBus, EventBus>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
