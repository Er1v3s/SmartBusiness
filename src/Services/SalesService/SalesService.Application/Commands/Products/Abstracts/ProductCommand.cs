namespace SalesService.Application.Commands.Products.Abstracts
{
    public abstract record ProductCommand(string Name, string Description, List<string> Category, decimal Price, int Tax, string ImageFile);
}

