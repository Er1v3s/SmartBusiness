namespace SmartBusiness.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
