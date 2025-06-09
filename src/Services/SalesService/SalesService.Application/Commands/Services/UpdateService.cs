using AutoMapper;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Abstracts;
using SalesService.Domain.Entities;
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
        public string Id { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateServiceCommandValidator : ServiceCommandValidator<UpdateServiceCommand> {}

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
            var serviceToUpdate = await _serviceRepository.GetServiceByIdAsync(request.Id);
            if (serviceToUpdate == null)
                throw new NotFoundException($"Service with id {request.Id} not found");

            if (serviceToUpdate.CompanyId != request.CompanyId)
                throw new ForbiddenException("You are not able to update service from other company than you are register in");

            var updatedService = _mapper.Map<Service>(request);
            await _serviceRepository.UpdateServiceAsync(serviceToUpdate, updatedService);

            return updatedService;
        }
    }
}
