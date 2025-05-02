namespace AuthService.Contracts.Requests.Users.Authentication
{
    public record LoginRequest(string Email, string Password);
}
