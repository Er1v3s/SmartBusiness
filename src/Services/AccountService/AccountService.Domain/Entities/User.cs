using System.ComponentModel.DataAnnotations;

namespace AccountService.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = null;
        public DateTime? RefreshTokenExpiresAtUtc { get; set; } = null;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public ICollection<UserCompanyRole> UserCompanyRoles { get; set; } = new List<UserCompanyRole>();

        public override string ToString()
        {
            return Email;
        }
    }
}
