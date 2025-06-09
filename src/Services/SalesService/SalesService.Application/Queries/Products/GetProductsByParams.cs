using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Abstracts;
using SalesService.Domain.Entities;
using Shared.Abstracts;

namespace SalesService.Application.Queries.Products
{
    public record GetProductsByParamsQuery(string? Id, string? Name, string? Category, decimal? MinPrice, decimal? MaxPrice) 
        : IRequest<IEnumerable<Product>>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetProductsByParamsQueryValidator : AbstractValidator<GetProductsByParamsQuery>
    {
        public GetProductsByParamsQueryValidator()
        {
            RuleFor(x => x.Name)
                .Must(Validator.BeValidString)
                .WithMessage($"{nameof(Product.Name)} can only contain letters and numbers")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Category)
                .Must(Validator.BeValidString)
                .WithMessage($"{nameof(Product.Category)} can only contain letters and numbers")
                .When(x => !string.IsNullOrEmpty(x.Category));

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min price must be greater or equal to 0.")
                .When(x => x.MinPrice != null);

            RuleFor(x => x.MaxPrice)
                .GreaterThan(0)
                .WithMessage("Max price must be greater than 0.")
                .Must((command, maxPrice) => maxPrice >= command.MinPrice)
                .WithMessage("Max price must be greater than or equal to Min price.")
                .When(x => x.MaxPrice != null);

            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.CompanyId)} is required.")
                .Length(17)
                .WithMessage($"{nameof(Service.CompanyId)} must be exactly 17 characters long.");
        }
    }

    public class GetProductsByParamsQueryHandler : IRequestHandler<GetProductsByParamsQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByParamsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsByParamsQuery request, CancellationToken cancellationToken)
        {
            var query = _productRepository.GetQueryable();

            // IMPORTANT STATEMENT (User can only get products from declared company)
            query = query.Where(p => p.CompanyId == request.CompanyId);

            query = ApplyFilters(query, request);

            var products = await _productRepository.GetFilteredProductsAsync(query, cancellationToken);

            return products;
        }

        private IQueryable<Product> ApplyFilters(IQueryable<Product> query, GetProductsByParamsQuery request)
        {
            if (!string.IsNullOrEmpty(request.Id))
                query = query.Where(p => p.Id == request.Id);

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(p => p.Name.Contains(request.Name));

            if (!string.IsNullOrEmpty(request.Category))
                query = query.Where(p => p.Category.Contains(request.Category));

            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice.Value);

            return query;
        }
    }
}
