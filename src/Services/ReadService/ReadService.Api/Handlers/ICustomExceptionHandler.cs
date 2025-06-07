using Microsoft.AspNetCore.Diagnostics;

namespace ReadService.Api.Handlers
{   
    public interface ICustomExceptionHandler : IExceptionHandler
    {
        bool CanHandle(Exception ex);
    }
}
