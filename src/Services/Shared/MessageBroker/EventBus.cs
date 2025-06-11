using MassTransit;
using Shared.Abstracts;

namespace Shared.MessageBroker
{
    public class EventBus : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class =>
            await _publishEndpoint.Publish(message, cancellationToken);
    }
}
