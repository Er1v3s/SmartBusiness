namespace AccountService.Contracts.Requests.CompanyRole
{
    public record CreateCompanyRoleRequest(Guid UserId, string Name);
}