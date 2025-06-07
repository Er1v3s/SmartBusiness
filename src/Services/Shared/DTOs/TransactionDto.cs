namespace Shared.DTOs
{
    public class TransactionDto
    {
        public string Id { get; set; } = null!;

        public string CompanyId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
