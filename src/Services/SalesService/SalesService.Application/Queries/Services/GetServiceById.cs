using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;
using Shared.Exceptions;

namespace SalesService.Application.Queries.Services
{
    public record GetServicesByIdQuery(string ServiceId) : IRequest<Service>;

    public class GetServicesByIdQueryHandler : IRequestHandler<GetServicesByIdQuery, Service>
    {
        private readonly IServiceRepository _serviceRepository;

        public GetServicesByIdQueryHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Service> Handle(GetServicesByIdQuery request, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(request.ServiceId);
            if(service == null)
                throw new NotFoundException($"Services with id: {request.ServiceId} not found.");

            return service;
        }
    }
}
