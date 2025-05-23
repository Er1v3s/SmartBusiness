namespace AccountService.Contracts.Requests.Account
{   
    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
}
