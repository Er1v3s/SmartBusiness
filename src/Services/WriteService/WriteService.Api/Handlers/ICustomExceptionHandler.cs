using Microsoft.AspNetCore.Diagnostics;

namespace WriteService.Api.Handlers
{   
    public interface ICustomExceptionHandler : IExceptionHandler
    {
        bool CanHandle(Exception ex);
    }
}
