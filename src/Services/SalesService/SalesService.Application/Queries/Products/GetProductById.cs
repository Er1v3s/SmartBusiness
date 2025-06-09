using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;
using Shared.Abstracts;
using Shared.Exceptions;

namespace SalesService.Application.Queries.Products
{
    public record GetProductsByIdQuery(string ProductId) : IRequest<Product>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetProductsByIdQueryHandler : IRequestHandler<GetProductsByIdQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(GetProductsByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);
            if(product == null)
                throw new NotFoundException($"Product with id: {request.ProductId} not found.");

            if (product.CompanyId != request.CompanyId)
                throw new ForbiddenException("You are not able to get product from company where you are not registered in");

            return product;
        }
    }
}
