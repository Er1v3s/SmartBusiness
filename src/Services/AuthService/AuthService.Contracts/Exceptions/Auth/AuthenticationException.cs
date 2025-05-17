namespace AuthService.Contracts.Exceptions.Auth
{
    public class AuthenticationException(string? message) : Exception(message);
}
