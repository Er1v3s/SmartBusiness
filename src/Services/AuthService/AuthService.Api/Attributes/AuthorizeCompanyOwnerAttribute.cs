using System.Security.Claims;
using AuthService.Application.Abstracts;
using AuthService.Contracts.Exceptions.Auth;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthService.Api.Attributes
{
    public class AuthorizeCompanyOwnerAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly ICompanyRepository _companyRepository;

        public AuthorizeCompanyOwnerAttribute(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var companyId = ExtractCompanyIdFromRequest(context.HttpContext);

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(companyId))
                throw new ForbiddenException("User should be owner of the company");

            var userId = Guid.Parse(userIdClaim);
            var isOwner = await _companyRepository.IsUserOwnerOfCompanyAsync(userId, companyId);
            if (!isOwner)
                throw new ForbiddenException("User should be owner of the company");
        }

        private static string ExtractCompanyIdFromRequest(HttpContext httpContext)
        {
            var companyId = httpContext.Request.RouteValues["companyId"]?.ToString();
            return string.IsNullOrEmpty(companyId) ? string.Empty : companyId;
        }
    }
}
