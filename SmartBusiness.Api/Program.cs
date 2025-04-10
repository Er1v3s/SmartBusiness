using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using SmartBusiness.Api.Handlers;
using SmartBusiness.Application;
using SmartBusiness.Infrastructure;

namespace SmartBusiness.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                    .AddService("SmartBusiness.Api")) // Nazwa serwisu widoczna w metrykach
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(builder.Configuration["Otel:Endpoint"]);
                    }));

            builder.Services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<SmartBusinessDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString"));
            });

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policyBuilder =>
                {
                    policyBuilder.AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddApplication();
            builder.Services.AddExceptionHandler<ExceptionHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();

                try
                {
                    using var serviceScope = app.Services.CreateScope();
                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<SmartBusinessDbContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception)
                {
                    Console.WriteLine("info: Cannot execute migrations. Database is already up to date");
                }
            }

            app.UseExceptionHandler(_ => { });

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
