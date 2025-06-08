namespace SalesService.Application.Commands.Abstracts
{
    public abstract record ServiceCommand(string Name, string Description, string Category, decimal Price, int Tax, int Duration);
}

