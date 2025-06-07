using Microsoft.Extensions.Hosting;

namespace ReadService.Infrastructure.Messaging
{
    public class RabbitMqConsumerHostedService : BackgroundService
    {
        private readonly RabbitMqConsumer _consumer;

        public RabbitMqConsumerHostedService(RabbitMqConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.StartListeningAsync(stoppingToken);
        }
    }
}
