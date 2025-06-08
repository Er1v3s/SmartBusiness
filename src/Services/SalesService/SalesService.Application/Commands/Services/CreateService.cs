using AutoMapper;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Abstracts;
using SalesService.Domain.Entities;

namespace SalesService.Application.Commands.Services
{
    public record CreateServiceCommand(string Name, string Description, string Category, decimal Price, int Tax, int Duration) 
        : ServiceCommand(Name, Description, Category, Price, Tax, Duration), IRequest<Service>;

    public class CreateServiceCommandValidator : ServiceCommandValidator<CreateServiceCommand>;

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
