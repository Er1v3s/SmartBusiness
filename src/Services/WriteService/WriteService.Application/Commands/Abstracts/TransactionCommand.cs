namespace WriteService.Application.Commands.Abstracts
{
    public record TransactionCommand(string ProductId, int Quantity, decimal TotalAmount, int Tax);
}
