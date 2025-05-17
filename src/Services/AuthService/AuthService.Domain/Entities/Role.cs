using NanoidDotNet;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities
{
    public class Role(string name)
    {
        [Key]
        public string Id { get; set; } = Nanoid.Generate(size: 11);
        public string Name { get; set; } = name;
        public ICollection<UserCompanyRole> UserCompanyRoles { get; set; } = new List<UserCompanyRole>();
    }
}
