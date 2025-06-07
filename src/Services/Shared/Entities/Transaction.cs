using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Entities
{
    public class Transaction
    {
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
        public decimal TotalAmount { get; set; }

        [BsonElement("timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
