using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;
using Shared.Abstracts;
using Shared.Exceptions;

namespace SalesService.Application.Commands.Products
{
    public record DeleteProductCommand(string ProductId) : IRequest<Unit>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.Id)} is required.");

            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Product.CompanyId)} is required.");
        }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);
            if (product == null)
                throw new NotFoundException($"Product with id {request.ProductId} not found");

            if (product.CompanyId != request.CompanyId)
                throw new ForbiddenException("You are not able to delete product from other company than you are register in");

            await _productRepository.DeleteProductAsync(product);

            return Unit.Value;
        }
    }
}
