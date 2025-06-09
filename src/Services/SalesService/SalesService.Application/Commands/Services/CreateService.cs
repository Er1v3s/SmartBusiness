using AutoMapper;
using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Abstracts;
using SalesService.Domain.Entities;

namespace SalesService.Application.Commands.Services
{
    public record CreateServiceCommand(
        string Name,
        string Description,
        string Category,
        decimal Price,
        int Tax,
        int Duration)
        : ServiceCommand(Name, Description, Category, Price, Tax, Duration), IRequest<Service>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateServiceCommandValidator : ServiceCommandValidator<CreateServiceCommand>
    {
        public CreateServiceCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.CompanyId)} is required.")
                .Length(17)
                .WithMessage($"{nameof(Service.CompanyId)} must be exactly 17 characters long.");
        }
    }

    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Service>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public CreateServiceCommandHandler(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<Service> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = _mapper.Map<Service>(request);
            await _serviceRepository.AddServiceAsync(service);

            return service;
        }
    }
}
