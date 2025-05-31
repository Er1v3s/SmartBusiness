namespace AccountService.Contracts.Requests.Auth
{
    public record ResetPasswordRequest(string Token, string NewPassword);
}
