using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using WriteService.Application.Abstracts;
using WriteService.Infrastructure.Messaging;
using WriteService.Infrastructure.Repositories;

namespace WriteService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672, UserName = "admin", Password = "admin" };
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });
            services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();

            return services;
        }
    }
}
