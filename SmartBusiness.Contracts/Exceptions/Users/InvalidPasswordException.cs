namespace AuthService.Contracts.Exceptions.Users
{
    public class InvalidPasswordException(string message) : Exception(message);
}
