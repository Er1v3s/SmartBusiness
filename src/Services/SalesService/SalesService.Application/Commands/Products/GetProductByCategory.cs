using FluentValidation;

namespace SalesService.Application.Commands.Products
{
    public record GetProductByCategoryCommand(string Category);
    public class GetProductByCategoryCommandValidator : AbstractValidator<GetProductByCategoryCommand>
    {
        public GetProductByCategoryCommandValidator()
        {
            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Category is required.");
        }
    }
    public class GetProductByCategory
    {
    }
}
