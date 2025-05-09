using FluentValidation;

namespace SalesService.Application.Commands.Products
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, List<string> Category, decimal Price);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.");
            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Category is required.");
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");
        }
    }

    public class UpdateProductCommandHandler
    {
    }
}
