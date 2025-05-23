using AccountService.Domain.Entities;

namespace AccountService.Contracts.DTOs
{
    public class UserCompanyRoleDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public RoleDto Role { get; set; } = new();

        public static UserCompanyRoleDto CreateDto(UserCompanyRole ucr)
        {
            return new UserCompanyRoleDto()
            {
                UserId = ucr.UserId,
                Username = ucr.User.Username,
                Email = ucr.User.Email,
                Role = new RoleDto()
                {
                    Id = ucr.Role.Id,
                    Name = ucr.Role.Name,
                }
            };
        }
    }
}
