using AutoMapper;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Abstracts;
using SalesService.Domain.Entities;
using Shared.Exceptions;

namespace SalesService.Application.Commands.Products
{
    public record UpdateProductCommand(
        string Name,
        string Description,
        string Category,
        decimal Price,
        int Tax)
        : ProductCommand(Name, Description, Category, Price, Tax), IRequest<Product>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class UpdateProductCommandValidator : ProductCommandValidator<UpdateProductCommand> {}

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        
        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productToUpdate = await _productRepository.GetProductByIdAsync(request.Id);
            if (productToUpdate == null)
                throw new NotFoundException($"Product with id {request.Id} not found");

            var updatedProduct = _mapper.Map<Product>(request);
            await _productRepository.UpdateProductAsync(productToUpdate, updatedProduct);

            return updatedProduct;
        }
    }
}
