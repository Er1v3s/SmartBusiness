namespace AuthService.Application.Commands.Users
{
    public abstract record UserCommand(string Username, string Email);
}
