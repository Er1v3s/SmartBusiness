using MassTransit;
using Shared.Abstracts;
using Shared.Contracts;

namespace ReadService.Infrastructure.Messaging
{
    public class CompanyDeletedEventConsumer : IConsumer<CompanyDeletedEvent>
    {
        private readonly ITransactionRepository _transactionRepository;

        public CompanyDeletedEventConsumer(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task Consume(ConsumeContext<CompanyDeletedEvent> context)
        {
            var companyId = context.Message.CompanyId;

            await _transactionRepository.DeleteManyAsync(companyId);
        }
    }
}