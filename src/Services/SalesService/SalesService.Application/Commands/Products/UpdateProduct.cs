using AutoMapper;
using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Abstracts;
using SalesService.Domain.Entities;
using Shared.Abstracts;
using Shared.Exceptions;

namespace SalesService.Application.Commands.Products
{
    public record UpdateProductCommand(
        string Name,
        string Description,
        string Category,
        decimal Price,
        int Tax)
        : ProductCommand(Name, Description, Category, Price, Tax), IRequest<Product>, IHaveCompanyId
    {
        public string ProductId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateProductCommandValidator : ProductCommandValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Id)} is required.");

            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.CompanyId)} is required.");
        }
    }

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
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);
            if (product == null)
                throw new NotFoundException($"Product with id {request.ProductId} not found");

            if (product.CompanyId != request.CompanyId)
                throw new ForbiddenException("You are not able to update product from other company than you are register in");

            _mapper.Map(request, product);
            await _productRepository.UpdateProductAsync(product);

            return product;
        }
    }
}
