using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Products.Abstracts;
using SalesService.Contracts.DTOs;

namespace SalesService.Application.Commands.Products
{
    public record UpdateProductCommand(string Id, string Name, string Description, List<string> Category, decimal Price, int Tax, string ImageFile) 
        : ProductCommand(Name, Description, Category, Price, Tax, ImageFile), IRequest<string>;

    public class UpdateProductCommandValidator : ProductCommandValidator<UpdateProductCommand> {}

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, string>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public async Task<string> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new ProductDto
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                Tax = request.Tax,
                ImageFile = request.ImageFile,
                UpdatedAt = DateTime.Now.ToUniversalTime(),
            };
            
            // await _productRepository.UpdateProductAsync(product);

            return product.Id;
        }
    }
}
