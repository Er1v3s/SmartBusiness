using AutoMapper;
using FluentValidation;
using MediatR;
using Shared.Contracts;
using Shared.DTOs;
using Shared.Entities;
using WriteService.Application.Abstracts;
using Shared.Abstracts;
using WriteService.Application.Commands.Abstracts;

namespace WriteService.Application.Commands.Transactions
{
    public record CreateTransactionCommand(
        string ProductId,
        int Quantity,
        decimal TotalAmount,
        int Tax
        ) : TransactionCommand(ProductId, Quantity, TotalAmount, Tax), IRequest<Transaction>, IHaveCompanyId, IHaveUserId
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
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IEventBus eventBus, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<Transaction> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = _mapper.Map<Transaction>(request);
            await _transactionRepository.AddAsync(transaction);

            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            await _eventBus.PublishAsync(new TransactionCreatedEvent(transactionDto), cancellationToken);

            return transaction;
        }
    }
}
