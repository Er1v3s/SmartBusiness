using AutoMapper;
using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Abstracts;
using SalesService.Domain.Entities;
using Shared.Abstracts;

namespace SalesService.Application.Commands.Products
{
    public record CreateProductCommand(
        string Name,
        string Description,
        string Category,
        decimal Price,
        int Tax)
        : ProductCommand(Name, Description, Category, Price, Tax), IRequest<Product>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateProductCommandValidator : ProductCommandValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.CompanyId)} is required.");
        }
    }

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
            await _productRepository.AddProductAsync(product);

            return product;
        }
    }
}
