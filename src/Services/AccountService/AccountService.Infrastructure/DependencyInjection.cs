using Microsoft.Extensions.DependencyInjection;
using AccountService.Application.Abstracts;
using AccountService.Infrastructure.Processors;
using AccountService.Infrastructure.Repositories;
using AccountService.Infrastructure.Services;
using Shared.Abstracts;
using Shared.MessageBroker;

namespace AccountService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IEventBus, EventBus>();
            services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();
            services.AddScoped<IResetPasswordTokenProcessor, ResetPasswordTokenProcessor>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUserCompanyRoleRepository, UserCompanyRoleRepository>();

            return services;
        }
    }
}
