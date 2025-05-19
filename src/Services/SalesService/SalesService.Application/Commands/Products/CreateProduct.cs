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
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            //await _productRepository.AddProductAsync(product);

            return product;
        }
    }
}
