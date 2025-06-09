using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;
using Shared.Abstracts;
using Shared.Exceptions;

namespace SalesService.Application.Commands.Services
{
    public record DeleteServiceCommand(string ServiceId) : IRequest<Unit>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class DeleteServiceCommandValidator : AbstractValidator<DeleteServiceCommand>
    {
        public DeleteServiceCommandValidator()
        {
            RuleFor(x => x.ServiceId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.Id)} is required.");

            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.CompanyId)} is required.");
        }
    }

    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, Unit>
    {
        private readonly IServiceRepository _serviceRepository;

        public DeleteServiceCommandHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Unit> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var existingService = await _serviceRepository.GetServiceByIdAsync(request.ServiceId);
            if (existingService == null)
                throw new NotFoundException($"Service with id {request.ServiceId} not found");

            if (existingService.CompanyId != request.CompanyId)
                throw new ForbiddenException("You are not able to delete service from other company than you are register in");

            await _serviceRepository.DeleteServiceAsync(existingService);

            return Unit.Value;
        }
    }
}
