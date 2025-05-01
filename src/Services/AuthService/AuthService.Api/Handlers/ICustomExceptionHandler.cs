using Microsoft.AspNetCore.Diagnostics;

namespace AuthService.Api.Handlers
{   
    public interface ICustomExceptionHandler : IExceptionHandler
    {
        bool CanHandle(Exception ex);
    }
}
