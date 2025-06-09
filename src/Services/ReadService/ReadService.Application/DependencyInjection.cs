using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using Shared.Mappings;
using System.Reflection;

namespace ReadService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CompanyIdBehavior<,>));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(TransactionMappingProfile));
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}