using AutoMapper;
using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Abstracts;
using SalesService.Domain.Entities;
using Shared.Abstracts;
using Shared.Exceptions;

namespace SalesService.Application.Commands.Services
{
    public record UpdateServiceCommand(
        string Name,
        string Description,
        string Category,
        decimal Price,
        int Tax,
        int Duration)
        : ServiceCommand(Name, Description, Category, Price, Tax, Duration), IRequest<Service>, IHaveCompanyId
    {
        public string ServiceId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateServiceCommandValidator : ServiceCommandValidator<UpdateServiceCommand>
    {
        public UpdateServiceCommandValidator()
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

    public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, Service>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public UpdateServiceCommandHandler(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }
        
        public async Task<Service> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(request.ServiceId);
            if (service == null)
                throw new NotFoundException($"Service with id {request.ServiceId} not found");

            if (service.CompanyId != request.CompanyId)
                throw new ForbiddenException("You are not able to update service from other company than you are register in");

            _mapper.Map(request, service);
            await _serviceRepository.UpdateServiceAsync(service);

            return service;
        }
    }
}
