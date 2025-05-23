namespace AccountService.Contracts.Exceptions.Users
{
    public class UserAlreadyExistsException(string message = "User already exist") : Exception(message) { }
}
