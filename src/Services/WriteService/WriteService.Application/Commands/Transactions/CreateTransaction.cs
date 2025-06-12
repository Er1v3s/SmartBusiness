using AutoMapper;
using FluentValidation;
using MediatR;
using Shared.Contracts;
using Shared.DTOs;
using Shared.Entities;
using Shared.Abstracts;
using WriteService.Application.Commands.Abstracts;

namespace WriteService.Application.Commands.Transactions
{
    public record CreateTransactionCommand(
        string ItemId,
        string ItemType,
        int Quantity,
        decimal TotalAmount,
        int Tax
        ) : TransactionCommand(ItemId, ItemType, Quantity, TotalAmount, Tax), IRequest<Transaction>, IHaveCompanyId, IHaveUserId
    {
        public string CompanyId { get; set; } = string.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
    }

    public class CreateTransactionCommandValidator : TransactionCommandValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Transaction.CompanyId)} is required.");

            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Transaction.UserId)} is required.");
        }
    }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Transaction>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IRedisCacheService redisCacheService, IEventBus eventBus, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _redisCacheService = redisCacheService;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<Transaction> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = _mapper.Map<Transaction>(request);
            await _transactionRepository.AddAsync(transaction);

            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            await _eventBus.PublishAsync(new TransactionCreatedEvent(transactionDto), cancellationToken);

            var cacheKey = $"transactions:{request.CompanyId}";
            await _redisCacheService.RemoveAsync(cacheKey);

            return transaction;
        }
    }
}
