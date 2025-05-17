using AuthService.Api.Handlers.CustomExceptionHandlers;

namespace AuthService.Api.Handlers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddScoped<ICustomExceptionHandler, GenericExceptionHandler>();
            services.AddScoped<ICustomExceptionHandler, UserExceptionHandler>();
            services.AddScoped<ICustomExceptionHandler, AuthenticationExceptionHandler>();

            return services;
        }
    }
}
