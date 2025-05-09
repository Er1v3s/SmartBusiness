using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;

namespace SalesService.Application.Commands.Products
{
    public record CreateProductCommand(string Name, string Description, List<string> Category, decimal Price, int Tax, string ImageFile) : IRequest<Product>;

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Name)} is required.")
                .MinimumLength(3)
                .WithMessage($"{nameof(Product.Name)} must be at least 3 characters long.")
                .MaximumLength(100)
                .WithMessage($"{nameof(Product.Name)} cannot be longer than 100 characters.");
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Description)} is required")
                .MinimumLength(3)
                .WithMessage($"{nameof(Product.Description)} must be at least 3 characters long.")
                .MaximumLength(500)
                .WithMessage($"{nameof(Product.Description)} cannot be longer than 500 characters.");
            RuleFor(x => x.Category)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Category)} is required.")
                .Must(c => c.Count > 0)
                .WithMessage($"{nameof(Product.Category)} must have at least 1 value.")
                .ForEach(c => c
                    .NotNull()
                    .NotEmpty()
                    .WithMessage($"{nameof(Product.Category)} cannot be null or empty.")
                    .MinimumLength(3)
                    .WithMessage($"{nameof(Product.Category)} must be at least 3 characters long.")
                    .MaximumLength(50)
                    .WithMessage($"{nameof(Product.Category)} cannot be longer than 50 characters."));
            RuleFor(x => x.Price)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Price)} is required.")
                .GreaterThan(0)
                .WithMessage($"{nameof(Product.Price)} must be greater than 0.")
                .PrecisionScale(7, 2, false)
                .WithMessage($"{nameof(Product.Price)} must have up to 2 decimal places and a maximum of 5 digits.");
            RuleFor(x => x.Tax)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Tax)} is required.")
                .InclusiveBetween(0, 100)
                .WithMessage($"{nameof(Product.Tax)} must be greater than 0 and less than 100.");
        }
    }

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
