namespace SalesService.Application.Commands.Abstracts
{
    public abstract record ProductCommand(string Name, string Description, string Category, decimal Price, int Tax);
}

