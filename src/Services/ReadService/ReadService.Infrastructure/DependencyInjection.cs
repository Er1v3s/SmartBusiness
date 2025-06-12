using Microsoft.Extensions.DependencyInjection;
using Shared.Abstracts;
using Shared.Cache;
using Shared.MessageBroker;
using Shared.Repositories;

namespace ReadService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IEventBus, EventBus>();
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
