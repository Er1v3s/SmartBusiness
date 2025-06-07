using AutoMapper;
using MassTransit;
using Shared.Abstracts;
using Shared.Contracts;

namespace ReadService.Infrastructure.Messaging
{
    public class TransactionDeletedEventConsumer : IConsumer<TransactionDeletedEvent>
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionDeletedEventConsumer(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task Consume(ConsumeContext<TransactionDeletedEvent> context)
        {
            var transactionToDeleteId = context.Message.TransactionToDeleteId;

            await _transactionRepository.DeleteAsync(transactionToDeleteId);
        }
    }
}
