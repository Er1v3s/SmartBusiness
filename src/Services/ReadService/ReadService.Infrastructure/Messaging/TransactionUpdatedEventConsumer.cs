using AutoMapper;
using MassTransit;
using Shared.Abstracts;
using Shared.Contracts;
using Shared.Entities;

namespace ReadService.Infrastructure.Messaging
{
    public class TransactionUpdatedEventConsumer : IConsumer<TransactionUpdatedEvent> 
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public TransactionUpdatedEventConsumer(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }
        public async Task Consume(ConsumeContext<TransactionUpdatedEvent> context)
        {
            var transactionDto = context.Message.TransactionDto;
            var transactionToUpdateId = transactionDto.Id;

            var transaction = _mapper.Map<Transaction>(transactionDto);

            await _transactionRepository.UpdateAsync(transactionToUpdateId, transaction);
        }
    }

}
