using Microsoft.AspNetCore.Diagnostics;

namespace SmartBusiness.Api.Handlers
{
    public interface ICustomExceptionHandler : IExceptionHandler
    {
        bool CanHandle(Exception ex);
    }
}
