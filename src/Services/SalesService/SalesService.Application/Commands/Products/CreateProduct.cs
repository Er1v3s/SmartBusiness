using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Products.Abstracts;
using SalesService.Domain.Entities;

namespace SalesService.Application.Commands.Products
{
    public record CreateProductCommand(string Name, string Description, List<string> Category, decimal Price, int Tax, string ImageFile) 
        : ProductCommand(Name, Description, Category, Price, Tax, ImageFile), IRequest<Product> { }

    public class CreateProductCommandValidator : ProductCommandValidator<CreateProductCommand> { }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                Tax = request.Tax,
                ImageFile = request.ImageFile
            };
            //await _productRepository.AddProductAsync(product);

            return product;
        }
    }
}
