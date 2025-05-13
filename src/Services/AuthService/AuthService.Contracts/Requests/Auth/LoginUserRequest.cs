namespace AuthService.Contracts.Requests.Auth
{
    public record LoginUserRequest(string Email, string Password);
}
