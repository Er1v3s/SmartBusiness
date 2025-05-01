using Microsoft.AspNetCore.Diagnostics;

namespace SmartBusiness.Auth.Handlers
{
    public interface ICustomExceptionHandler : IExceptionHandler
    {
        bool CanHandle(Exception ex);
    }
}
