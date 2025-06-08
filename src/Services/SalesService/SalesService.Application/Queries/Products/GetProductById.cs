using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;
using Shared.Exceptions;

namespace SalesService.Application.Queries.Products
{
    public record GetProductsByIdQuery(string ProductId) : IRequest<Product>;

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

            return product;
        }
    }
}
