namespace AccountService.Contracts.Requests.Auth
{
    public record LoginUserRequest(string Email, string Password);
}
