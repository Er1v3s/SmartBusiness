namespace SmartBusiness.Contracts.Requests.Authentication
{
    public record CreateUserRequest(string Username, string Email, string Password);
}
