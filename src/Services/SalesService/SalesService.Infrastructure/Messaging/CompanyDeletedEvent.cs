using MassTransit;
using SalesService.Application.Abstracts;
using Shared.Contracts;

namespace SalesService.Infrastructure.Messaging
{
    public class CompanyDeletedEventConsumer : IConsumer<CompanyDeletedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly IServiceRepository _serviceRepository;

        public CompanyDeletedEventConsumer(IProductRepository productRepository, IServiceRepository serviceRepository)
        {
            _productRepository = productRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task Consume(ConsumeContext<CompanyDeletedEvent> context)
        {
            var companyId = context.Message.CompanyId;

            await _productRepository.DeleteManyProductsByCompanyIdAsync(companyId);
            await _serviceRepository.DeleteManyServicesByCompanyIdAsync(companyId);
        }
    }
}