using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ReadService.Api.Handlers;
using ReadService.Api.Handlers.CustomExceptionHandlers;
using ReadService.Application;
using ReadService.Infrastructure;
using ReadService.Infrastructure.Messaging;
using Shared.Settings;

namespace ReadService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region database connection

            builder.Services.Configure<MongoDbSettings>(
                builder.Configuration.GetSection("ReadMongoDbSettings"));

            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                
                var client = new MongoClient(settings.ConnectionString);
                return client.GetDatabase(settings.DatabaseName);
            });

            #endregion

            #region Message Broker Consumer (RabbitMQ)

            builder.Services.Configure<MessageBrokerSettings>(
                builder.Configuration.GetSection("MessageBroker"));

            builder.Services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            builder.Services.AddMassTransit(busConfiguration =>
            {
                busConfiguration.SetKebabCaseEndpointNameFormatter();

                busConfiguration.AddConsumer<TransactionCreatedEventConsumer>();
                busConfiguration.AddConsumer<TransactionUpdatedEventConsumer>();
                busConfiguration.AddConsumer<TransactionDeletedEventConsumer>();

                busConfiguration.UsingRabbitMq((context, configurator) =>
                {
                    MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

                    configurator.Host(new Uri(settings.HostName), h =>
                    {
                        h.Username(settings.UserName);
                        h.Password(settings.Password);
                    });

                    configurator.ReceiveEndpoint("transaction-created", e =>
                    {
                        e.ConfigureConsumer<TransactionCreatedEventConsumer>(context);
                    });

                    configurator.ReceiveEndpoint("transaction-updated", e =>
                    {
                        e.ConfigureConsumer<TransactionUpdatedEventConsumer>(context);
                    });

                    configurator.ReceiveEndpoint("transaction-deleted", e =>
                    {
                        e.ConfigureConsumer<TransactionDeletedEventConsumer>(context);
                    });
                });
            });

            #endregion

            #region http policy

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins("http://localhost:5000")
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            #endregion

            #region metrics

            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService("read.smart-business"))
                .WithTracing(tracking => tracking
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter()
                )
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddPrometheusExporter()
                );

            #endregion

            #region exceptions handling

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddScoped<ICustomExceptionHandler, GenericExceptionHandler>();

            #endregion

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHealthChecks();
            builder.Services.AddControllers();

            #region api documentation

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SmartBusiness.ReadService API",
                    Version = "v1",
                    Description = "SmartBusiness.ReadService API",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string> {}
                    }
                });
            });

            #endregion

            var app = builder.Build();

            #region dev tools

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("http://localhost:2400/swagger/v1/swagger.json", "SmartBusiness.ReadService API");
                    c.RoutePrefix = "swagger";
                });
            }

            #endregion

            // DO NOT CHANGE ORDER !!!
            app.UseExceptionHandler(_ => { });

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapPrometheusScrapingEndpoint();
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.MapControllers();

            app.Run();
        }
    }
}
