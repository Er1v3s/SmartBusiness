using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using WriteService.Application.Abstracts;
using WriteService.Domain.Entities;

namespace WriteService.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IRabbitMqPublisher, IAsyncDisposable
    {
        private readonly IConnection _connection;

        public RabbitMqPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public async Task PublishTransactionEvent(Transaction transaction)
        {
            await using var channel = await _connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "transactions_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = JsonSerializer.Serialize(transaction);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "transactions_queue",
                mandatory: true,
                basicProperties: new BasicProperties { Persistent = true },
                body: body);
        }

        public ValueTask DisposeAsync()
        {
            _connection?.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}