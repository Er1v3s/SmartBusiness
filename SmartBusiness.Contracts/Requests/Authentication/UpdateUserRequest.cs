namespace SmartBusiness.Contracts.Requests.Authentication
{
    public record UpdateUserRequest(Guid Id, string Username, string Email);
}
