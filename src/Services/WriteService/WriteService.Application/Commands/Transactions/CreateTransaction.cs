using AutoMapper;
using FluentValidation;
using MediatR;
using Shared.Contracts;
using Shared.DTOs;
using Shared.Entities;
using WriteService.Application.Abstracts;
using Shared.Abstracts;
using ITransactionRepository = Shared.Abstracts.ITransactionRepository;

namespace WriteService.Application.Commands.Transactions
{
    public record CreateTransactionCommand(
        string CompanyId,
        string UserId,
        string ProductId,
        int Quantity,
        decimal TotalAmount) : IRequest<string>;

    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required.");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than zero.");
        }
    }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, string>
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

        public async Task<string> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                CompanyId = request.CompanyId,
                UserId = request.UserId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                TotalAmount = request.TotalAmount,
                CreatedAt = DateTime.UtcNow
            };

            var transactionDto = _mapper.Map<TransactionDto>(transaction);

            await _transactionRepository.AddAsync(transaction);
            await _eventBus.PublishAsync(new TransactionCreatedEvent(transactionDto), cancellationToken);

            return transaction.Id;
        }
    }
}
