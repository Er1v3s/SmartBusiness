﻿using Microsoft.AspNetCore.Diagnostics;
using SalesService.Api.Handlers.CustomExceptionHandlers;

namespace SalesService.Api.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public GlobalExceptionHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
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
