namespace SmartBusiness.Application.Commands.Authentication
{
    public abstract record UserCommand(string Username, string Email);
}
