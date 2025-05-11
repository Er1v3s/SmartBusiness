using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;

namespace SalesService.Application.Commands.Products
{
    public record GetAllProductsCommand : IRequest<List<Product>> { }

    public class GetAllProductsCommandHandler : IRequestHandler<GetAllProductsCommand, List<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<List<Product>> Handle(GetAllProductsCommand request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetAllProductsAsync(cancellationToken);
        }
    }
}
