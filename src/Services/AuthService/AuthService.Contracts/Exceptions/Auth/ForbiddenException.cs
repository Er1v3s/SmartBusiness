namespace AuthService.Contracts.Exceptions.Auth
{
    public class ForbiddenException(string? message) : Exception(message);
}
