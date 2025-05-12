using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;
using Shared.Exceptions;

namespace SalesService.Application.Commands.Products
{
    public record DeleteProductCommand(Guid Id) : IRequest<Product>;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Product>
    {
        private readonly IProductRepository _productRepository;
        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Product> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            //var product = await _productRepository.GetProductByIdAsync(request.Id);
            //if (product == null)
                //throw new NotFoundException($"Product with ID {request.Id} not found.");

            //await _productRepository.DeleteProductAsync(product);

            return new Product();
        }
    }
}
