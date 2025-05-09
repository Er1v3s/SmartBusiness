using AuthService.Application.Abstracts;
using AuthService.Infrastructure.Processors;
using AuthService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
