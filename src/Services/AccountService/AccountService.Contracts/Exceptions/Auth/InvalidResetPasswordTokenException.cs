namespace AccountService.Contracts.Exceptions.Auth
{
    public class InvalidResetPasswordTokenException(string message) : Exception(message);
}
