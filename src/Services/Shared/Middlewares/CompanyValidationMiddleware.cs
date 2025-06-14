﻿using Microsoft.AspNetCore.Http;

namespace Shared.Middlewares
{
    public class CompanyValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public CompanyValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_excludePaths.Contains(context.Request.Path.Value))
            {
                await _next(context);

                return;
            }

            var user = context.User;
            var companyId = context.Request.Headers["X-Company-Id"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(companyId))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Missing company identifier.");

                return;
            }

            var claims = user.FindAll("companyId").Select(c => c.Value).ToList();

            if (!claims.Contains(companyId))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("User not authorized for the selected company.");

                return;
            }

            context.Items["CompanyId"] = companyId;

            await _next(context);
        }

        private static readonly HashSet<string> _excludePaths = new () 
        {
            "/health",
            "/metrics",
            "/swagger",
            "/swagger/v1/swagger.json",
            "/swagger/v1/swagger.yaml"
        };
    }
}
