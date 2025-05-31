namespace AccountService.Application.Abstracts
{
    public interface IResetPasswordTokenProcessor
    {
        string GenerateResetPasswordToken();
    }
}
