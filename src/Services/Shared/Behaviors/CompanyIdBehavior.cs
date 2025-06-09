using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Abstracts;

namespace Shared.Behaviors
{
    public class CompanyIdBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyIdBehavior(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IHaveCompanyId companyScoped)
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context.Request.Headers["X-Company-Id"].FirstOrDefault();

                if (!string.IsNullOrEmpty(companyId))
                {
                    companyScoped.CompanyId = companyId;
                }
            }

            return await next(cancellationToken);
        }
    }
}
