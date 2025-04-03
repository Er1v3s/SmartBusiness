using System.ComponentModel.DataAnnotations;

namespace SmartBusiness.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime CreatedAt { get; } = DateTime.Now.ToUniversalTime();
    }
}
