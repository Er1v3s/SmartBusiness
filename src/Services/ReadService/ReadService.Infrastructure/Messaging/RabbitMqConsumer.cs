using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReadService.Domain.Entities;
using System.Text;
using System.Text.Json;

namespace ReadService.Infrastructure.Messaging
{
    public class RabbitMqConsumer :IAsyncDisposable
    {
        private readonly IMongoCollection<Transaction> _transactionsCollection;
        private readonly IConnection _connection;
        private IChannel? _channel;

        public RabbitMqConsumer(IMongoDatabase database, IConnection connection)
        {
            _transactionsCollection = database.GetCollection<Transaction>("transactions");
            _connection = connection;
        }

        public async Task StartListeningAsync(CancellationToken cancellationToken = default)
        {
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(
                queue: "transactions_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null, cancellationToken: cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var transaction = JsonSerializer.Deserialize<Transaction>(message);

                if (transaction != null)
                {
                    try
                    {
                        await _transactionsCollection.ReplaceOneAsync(
                            t => t.Id == transaction.Id,
                            transaction,
                            new ReplaceOptions { IsUpsert = true },
                            cancellationToken
                        );

                        await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false, cancellationToken: cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to process transaction {transaction.Id}: {ex.Message}");
                    }
                }
            };

            await _channel.BasicConsumeAsync(
                queue: "transactions_queue",
                autoAck: false,
                consumer: consumer,
                cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
                await _channel.CloseAsync();
            _connection?.Dispose();
        }
    }
}
