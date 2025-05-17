namespace AuthService.Contracts.DTOs
{
    public class UserCompanyRoleDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public RoleDto Role { get; set; }
    }
}
