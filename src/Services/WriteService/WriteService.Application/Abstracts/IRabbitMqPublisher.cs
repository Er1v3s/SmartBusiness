using WriteService.Domain.Entities;

namespace WriteService.Application.Abstracts
{
    public interface IRabbitMqPublisher
    {
        Task PublishTransactionEvent(Transaction transaction);
    }
}
