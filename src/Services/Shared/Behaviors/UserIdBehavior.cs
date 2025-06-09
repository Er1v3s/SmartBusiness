using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Abstracts;
using System.Security.Claims;

namespace Shared.Behaviors
{
    public class UserIdBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdBehavior(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is IHaveUserId companyScoped)
            {
                var context = _httpContextAccessor.HttpContext;
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    companyScoped.UserId = Guid.Parse(userId);
                }
            }

            return await next(cancellationToken);
        }
    }
}
