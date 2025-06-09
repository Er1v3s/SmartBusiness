using MediatR;
using Microsoft.AspNetCore.Http;
using SalesService.Application.Abstracts;

namespace SalesService.Application.Behaviors
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
                if (context != null && context.Items.TryGetValue("CompanyId", out var companyId))
                {
                    companyScoped.CompanyId = companyId?.ToString()!;
                }
            }

            return await next(cancellationToken);
        }
    }
}
