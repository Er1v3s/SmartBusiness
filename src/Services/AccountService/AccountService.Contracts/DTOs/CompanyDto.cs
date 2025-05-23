using AccountService.Domain.Entities;

namespace AccountService.Contracts.DTOs
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
                    .Select(ucr => new UserCompanyRoleDto
                    {
                        UserId = ucr.User.Id,
                        Username = ucr.User.Username,
                        Email = ucr.User.Email,
                        Role = new RoleDto
                        {
                            Id = ucr.Role.Id,
                            Name = ucr.Role.Name
                        }
                    }).ToList()
            };
        }
    }
}
