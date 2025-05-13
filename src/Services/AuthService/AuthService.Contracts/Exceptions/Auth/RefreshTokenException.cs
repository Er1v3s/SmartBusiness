namespace AuthService.Contracts.Exceptions.Auth
{
    public class RefreshTokenException(string? message) : Exception(message);
}
