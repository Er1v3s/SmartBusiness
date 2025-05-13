namespace AuthService.Application.Commands.Abstracts
{
    public abstract record UserCommand(string? Username, string? Email);
}
