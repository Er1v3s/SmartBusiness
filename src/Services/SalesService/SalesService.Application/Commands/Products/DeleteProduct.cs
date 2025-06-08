using MediatR;
using SalesService.Application.Abstracts;
using Shared.Exceptions;

namespace SalesService.Application.Commands.Products
{
    public record DeleteProductCommand(string ProductId) : IRequest<Unit>;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(request.ProductId);
            if (existingProduct == null)
                throw new NotFoundException($"Product with id {request.ProductId} not found");

            await _productRepository.DeleteProductAsync(existingProduct);

            return Unit.Value;
        }
    }
}
