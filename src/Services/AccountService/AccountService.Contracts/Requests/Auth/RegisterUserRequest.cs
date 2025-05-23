namespace AccountService.Contracts.Requests.Auth
{
    public record RegisterUserRequest(string Username, string Email, string Password);
}
