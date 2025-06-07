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
            Console.WriteLine("Consumer działa jako BackgroundService!");
            await _consumer.StartListeningAsync(stoppingToken);
        }
    }
}
