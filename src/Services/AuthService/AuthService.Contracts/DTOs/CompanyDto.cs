using AuthService.Domain.Entities;

namespace AuthService.Contracts.DTOs
{
    public class CompanyDto
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<UserCompanyRoleDto> Users { get; set; } = new();


        public static CompanyDto CreateDto(Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                CreatedAt = company.CreatedAt,
                Users = company.UserCompanyRoles
                    .Select(u => new UserCompanyRoleDto
                    {
                        UserId = u.User.Id,
                        Username = u.User.Username,
                        Email = u.User.Email,
                        Role = new RoleDto
                        {
                            Id = u.Role.Id,
                            Name = u.Role.Name
                        }
                    }).ToList()
            };
        }
    }
}
