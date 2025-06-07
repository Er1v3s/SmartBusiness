using AutoMapper;
using MassTransit;
using Shared.Contracts;
using Shared.Abstracts;
using Shared.Entities;

namespace ReadService.Infrastructure.Messaging
{
    public class TransactionCreatedEventConsumer : IConsumer<TransactionCreatedEvent>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionCreatedEventConsumer(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }

        public async Task Consume(ConsumeContext<TransactionCreatedEvent> context)
        {
            var transactionDto = context.Message.TransactionDto;

            var transaction = _mapper.Map<Transaction>(transactionDto);

            await _transactionRepository.AddAsync(transaction);
        }
    }
}
