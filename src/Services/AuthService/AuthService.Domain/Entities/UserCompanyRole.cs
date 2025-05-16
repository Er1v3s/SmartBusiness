using System.Text.Json.Serialization;

namespace AuthService.Domain.Entities
{
    public class UserCompanyRole(Guid userId, string companyId, string roleId)
    {
        public Guid UserId { get; set; } = userId;
        [JsonIgnore]
        public virtual User User { get; set; } = null!;

        public string CompanyId { get; set; } = companyId;
        [JsonIgnore]
        public virtual Company Company { get; set; } = null!;

        public string RoleId { get; set; } = roleId;
        [JsonIgnore]
        public virtual Role Role { get; set; } = null!;
    }
}
