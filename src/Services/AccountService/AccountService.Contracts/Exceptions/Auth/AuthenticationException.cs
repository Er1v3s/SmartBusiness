namespace AccountService.Contracts.Exceptions.Auth
{
    public class AuthenticationException(string? message) : Exception(message);
}
