using Microsoft.Extensions.DependencyInjection;
using AccountService.Application.Abstracts;
using AccountService.Infrastructure.Processors;
using AccountService.Infrastructure.Repositories;

namespace AccountService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUserCompanyRoleRepository, UserCompanyRoleRepository>();

            return services;
        }
    }
}
