﻿using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using Shared.Mappings;
using System.Reflection;

namespace WriteService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(TransactionMappingProfile));


            return services;
        }
    }
}
