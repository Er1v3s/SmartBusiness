namespace SmartBusiness.Contracts.Requests.Authentication
{
    public record ChangeUserPasswordRequest(Guid Id, string CurrentPassword, string NewPassword);
}
