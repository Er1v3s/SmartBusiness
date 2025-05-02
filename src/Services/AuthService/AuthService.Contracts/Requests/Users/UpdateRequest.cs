namespace AuthService.Contracts.Requests.Users
{
    public record UpdateRequest(Guid Id, string Username, string Email);
}
