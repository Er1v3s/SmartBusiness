namespace AuthService.Contracts.Requests.Users
{
    public record CreateRequest(string Username, string Email, string Password);
}
