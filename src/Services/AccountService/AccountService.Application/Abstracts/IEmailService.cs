namespace AccountService.Application.Abstracts
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string htmlContent);
    }
}
