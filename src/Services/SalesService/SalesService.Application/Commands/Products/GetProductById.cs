using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;
using Shared.Exceptions;

namespace SalesService.Application.Commands.Products
{
    public record GetProductByIdCommand(Guid Id) : IRequest<Product>;

    public class GetProductByIdCommandHandler : IRequestHandler<GetProductByIdCommand, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<Product> Handle(GetProductByIdCommand request, CancellationToken cancellationToken)
        {
            var product = _productRepository.GetProductByIdAsync(request.Id);
            if (product == null)
                throw new NotFoundException($"Product with ID {request.Id} not found.");

            return product;
        }
    }
}
