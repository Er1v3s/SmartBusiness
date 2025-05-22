using System.ComponentModel.DataAnnotations;
using NanoidDotNet;

namespace AuthService.Domain.Entities
{
    public class Company(string name)
    {
        [Key]
        public string Id { get; set; } = Nanoid.Generate(size: 17);
        public string Name { get; set; } = name;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public ICollection<UserCompanyRole> UserCompanyRoles { get; set; } = new List<UserCompanyRole>();

        public override string ToString()
        {
            return Name;
        }
    }
}
