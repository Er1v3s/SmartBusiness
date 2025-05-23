using Microsoft.AspNetCore.Diagnostics;

namespace AccountService.Api.Handlers
{   
    public interface ICustomExceptionHandler : IExceptionHandler
    {
        bool CanHandle(Exception ex);
    }
}
