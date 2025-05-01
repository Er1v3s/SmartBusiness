using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AuthService.Tests.Helpers
{
    internal class FakeUserFilter : IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Admin")
            }));

            context.HttpContext.User = claimsPrincipal;

            return next();
        }
    }
}
