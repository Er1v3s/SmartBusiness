namespace AuthService.Contracts.Requests.Auth
{
    public record RegisterUserRequest(string Username, string Email, string Password);
}
