using AutoMapper;
using FluentValidation;
using MediatR;
using Shared.Abstracts;
using Shared.Contracts;
using Shared.DTOs;
using Shared.Entities;
using Shared.Exceptions;
using WriteService.Application.Commands.Abstracts;

namespace WriteService.Application.Commands.Transactions
{
    public record UpdateTransactionCommand(
        string ProductId,
        int Quantity,
        decimal TotalAmount,
        int Tax
    ) : TransactionCommand(ProductId, Quantity, TotalAmount, Tax), IRequest<bool>, IHaveCompanyId, IHaveUserId
    {
        public string TransactionId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
    }

    public class UpdateTransactionCommandValidator : TransactionCommandValidator<UpdateTransactionCommand>
    {
        public UpdateTransactionCommandValidator()
        {
            RuleFor(x => x.TransactionId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Transaction.Id)} is required.");

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
            var transaction = await _transactionRepository.GetTransactionByIdAsync(request.TransactionId);
            if (transaction == null)
                throw new NotFoundException($"Transaction with id {request.TransactionId} not found");

            if (transaction.CompanyId != request.CompanyId)
                throw new ForbiddenException("You are not able to update transaction from other company than you are register in");

            _mapper.Map(request, transaction);
            await _transactionRepository.UpdateAsync(transaction);

            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            await _eventBus.PublishAsync(new TransactionUpdatedEvent(transactionDto), cancellationToken);

            return true;
        }
    }
}
