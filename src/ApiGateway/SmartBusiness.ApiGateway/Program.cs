using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using HealthChecks.UI.Core;
using HealthChecks.UI.Configuration; // Add this namespace
using HealthChecks.UI;

namespace SmartBusiness.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            #region logging

            builder.Services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
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
                .ConfigureResource(resource => resource.AddService("gateway.smart-business"))
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

            #region api documentation

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SmartBusiness.ApiGateway",
                    Version = "v1",
                    Description = "SmartBusiness.ApiGateway",
                });
            });

            #endregion

            builder.Services.AddHealthChecks();

            builder.Services.AddHttpClient();
            builder.Services.AddControllers();

            var app = builder.Build();


            #region dev tools

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("http://localhost:2000/swagger/v1/swagger.json", "SmartBusiness.ApiGateway v1");
                    c.RoutePrefix = "swagger";
                });
            }

            #endregion

            // DO NOT CHANGE ORDER !!!
            app.UseCors("CorsPolicy");

            app.Use(async (context, next) =>
            {
                // Enable Prometheus metrics endpoint
                if (context.Request.Path.StartsWithSegments("/metrics"))
                {
                    await next();
                }
                else
                {
                    context.Request.Scheme = "https";
                    await next.Invoke();
                }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapPrometheusScrapingEndpoint();
            app.MapReverseProxy();
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
