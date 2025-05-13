using AuthService.Api.Handlers.CustomExceptionHandlers;
using Microsoft.AspNetCore.Diagnostics;

namespace AuthService.Api.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(IServiceProvider serviceProvider, ILogger<GlobalExceptionHandler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);

            using var scope = _serviceProvider.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<ICustomExceptionHandler>();

            var customExceptionHandlers = handlers as ICustomExceptionHandler[] ?? handlers.ToArray();
            foreach (var handler in customExceptionHandlers)
            {
                if (handler.CanHandle(exception))
                {
                    return await handler.TryHandleAsync(context, exception, cancellationToken);
                }
            }

            // Fallback
            var fallback = customExceptionHandlers.FirstOrDefault(h => h is GenericExceptionHandler);
            return fallback != null && await fallback.TryHandleAsync(context, exception, cancellationToken);
        }
    }
}
