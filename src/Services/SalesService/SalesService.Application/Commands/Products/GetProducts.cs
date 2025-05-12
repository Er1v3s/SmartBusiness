using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Products.Abstracts;
using SalesService.Domain.Entities;
using Shared.Exceptions;

namespace SalesService.Application.Commands.Products
{
    public record GetProductsCommand(string? Id, string? Name, string? Category, decimal? MinPrice, decimal? MaxPrice) 
        : IRequest<List<Product>>;

    public class GetFilteredProductsCommandValidator : AbstractValidator<GetProductsCommand>
    {
        public GetFilteredProductsCommandValidator()
        {
            RuleFor(x => x.Name)
                .Must(Validator.BeValidString)
                .WithMessage($"{nameof(Product.Name)} can only contain letters and numbers");

            RuleFor(x => x.Category)
                .Must(Validator.BeValidString)
                .WithMessage($"{nameof(Product.Category)} can only contain letters and numbers");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min price must be greater or equal to 0.");

            RuleFor(x => x.MaxPrice)
                .GreaterThan(0)
                .WithMessage("Max price must be greater than 0.")
                .Must((command, maxPrice) => maxPrice >= command.MinPrice)
                .WithMessage("Max price must be greater than or equal to Min price.");
        }
    }

    public class GetProductsCommandHandler : IRequestHandler<GetProductsCommand, List<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> Handle(GetProductsCommand request, CancellationToken cancellationToken)
        {
            var query = _productRepository.GetQueryable(cancellationToken);

            if(string.IsNullOrEmpty(request.Id))
                query = query.Where(p => p.Id == request.Id);

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(p => p.Name.Contains(request.Name));

            if (!string.IsNullOrEmpty(request.Category))
                query = query.Where(p => p.Category.Contains(request.Category));

            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice.Value);

            var products = await _productRepository.GetFilteredProductsAsync(query, cancellationToken);
            if(products == null || products.Count == 0)
                throw new NotFoundException($"Products with the specified filters not found.");

            return products;
        }
    }
}
