using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReadService.Domain.Entities;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Prometheus;

namespace ReadService.Infrastructure.Messaging
{
    public class RabbitMqConsumer :IAsyncDisposable
    {
        private static readonly ActivitySource ActivitySource = new("read.smart-business");
        private static readonly Counter FailedDbOperations =
            Metrics.CreateCounter("readservice_db_errors", "Liczba błędów operacji na bazie danych");


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
                using var activity = ActivitySource.StartActivity("ProcessingTransaction");

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

                        activity?.SetTag("transaction.id", transaction.Id);
                        activity?.SetTag("transaction.status", "Processed");

                        await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false, cancellationToken: cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        activity?.SetTag("transaction.id", transaction.Id);
                        activity?.SetTag("status", "failure");
                        activity?.SetTag("error.message", ex.Message);
                        activity?.SetTag("error.type", ex.GetType().Name);
                        FailedDbOperations.Inc();

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
