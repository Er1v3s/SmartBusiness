namespace SmartBusiness.Contracts.Errors
{
    public class NotFoundException(string message) : Exception(message);
}
