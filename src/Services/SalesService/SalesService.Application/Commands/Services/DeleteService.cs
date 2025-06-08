using MediatR;
using SalesService.Application.Abstracts;
using Shared.Exceptions;

namespace SalesService.Application.Commands.Services
{
    public record DeleteServiceCommand(string ServiceId) : IRequest<Unit>;

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

            await _serviceRepository.DeleteServiceAsync(existingService);

            return Unit.Value;
        }
    }
}
