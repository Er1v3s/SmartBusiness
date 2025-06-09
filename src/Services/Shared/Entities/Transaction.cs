using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Entities
{
    public class Transaction
    {
        private int _tax;
        private decimal _totalAmount;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("companyId")]
        public string CompanyId { get; set; } = null!;

        [BsonElement("userId")]
        public string UserId { get; set; } = null!;

        [BsonElement("productId")]
        public string ProductId { get; set; } = null!;

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("totalAmount")]
        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                CalculateTotalAmountMinusTax();
            }
        }

        [BsonElement("tax")]
        public int Tax
        {
            get => _tax;
            set
            {
                _tax = value;
                CalculateTotalAmountMinusTax();
            }
        }

        [BsonElement("TotalAmountMinusTax")]
        public decimal TotalAmountMinusTax { get; set; }

        [BsonElement("timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        private void CalculateTotalAmountMinusTax()
        {
            if (TotalAmount != 0 && Tax != 0)
            {
                decimal rawValue = TotalAmount - (TotalAmount * Tax / 100);
                decimal roundedValue = Math.Ceiling(rawValue * 100) / 100;

                TotalAmountMinusTax = roundedValue;
            }
        }
    }
}
