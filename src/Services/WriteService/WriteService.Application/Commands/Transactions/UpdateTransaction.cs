using AutoMapper;
using MediatR;
using Shared.Abstracts;
using Shared.Contracts;
using Shared.DTOs;
using Shared.Entities;
using WriteService.Application.Abstracts;
using WriteService.Domain.Entities;

namespace WriteService.Application.Commands.Transactions
{
    public record UpdateTransactionCommand(
        string Id,
        string CompanyId,
        string UserId,
        string ProductId,
        int Quantity,
        decimal TotalAmount
    ) : IRequest<bool>;

    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, bool>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository, IEventBus eventBus, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                Id = request.Id,
                CompanyId = request.CompanyId,
                UserId = request.UserId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                TotalAmount = request.TotalAmount,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.UpdateAsync(request.Id, transaction);

            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            await _eventBus.PublishAsync(new TransactionUpdatedEvent(transactionDto), cancellationToken);

            return true;
        }
    }
}
