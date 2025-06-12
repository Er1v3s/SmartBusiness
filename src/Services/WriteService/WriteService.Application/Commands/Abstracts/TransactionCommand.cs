namespace WriteService.Application.Commands.Abstracts
{
    public record TransactionCommand(string ItemId, string ItemType, int Quantity, decimal TotalAmount, int Tax);
}
