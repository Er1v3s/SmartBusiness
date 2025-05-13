namespace AuthService.Domain.Entities
{
    public class UserCompanyRole
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public string CompanyId { get; set; } = string.Empty;
        public virtual Company Company { get; set; } = null!;

        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
    }
}
