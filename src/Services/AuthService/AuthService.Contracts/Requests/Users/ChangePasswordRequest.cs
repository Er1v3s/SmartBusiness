namespace AuthService.Contracts.Requests.Users
{   
    public record ChangePasswordRequest(Guid Id, string CurrentPassword, string NewPassword);
}
