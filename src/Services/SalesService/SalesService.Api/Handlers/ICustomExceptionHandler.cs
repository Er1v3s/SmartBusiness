using Microsoft.AspNetCore.Diagnostics;

namespace SalesService.Api.Handlers
{   
    public interface ICustomExceptionHandler : IExceptionHandler
    {
        bool CanHandle(Exception ex);
    }
}
