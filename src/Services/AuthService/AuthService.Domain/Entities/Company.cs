using System.ComponentModel.DataAnnotations;
using NanoidDotNet;

namespace AuthService.Domain.Entities
{
    public class Company
    {
        [Key]
        public string Id { get; set; } = Nanoid.Generate(size: 17);
        public string Name { get; set; } = string.Empty;
        public int? CreatedByUserId { get; set; }
        public virtual User? CreatedByUser { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public ICollection<UserCompanyRole> UserCompanyRoles { get; set; } = new List<UserCompanyRole>();
    }
}
