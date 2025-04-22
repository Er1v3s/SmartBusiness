using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using SmartBusiness.Api.Handlers;
using SmartBusiness.Api.Handlers.CustomExceptionHandlers;
using SmartBusiness.Application;
using SmartBusiness.Application.Abstracts;
using SmartBusiness.Infrastructure;
using SmartBusiness.Infrastructure.Options;
using SmartBusiness.Infrastructure.Processors;
using SmartBusiness.Infrastructure.Repositories;

namespace SmartBusiness.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region database connection

            builder.Services.AddDbContext<SmartBusinessDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString"));
            });

            #endregion

            #region http policy

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

            #endregion

            #region authentication

            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtOptions = builder.Configuration.GetSection(JwtOptions.JwtOptionsKey)
                    .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["ACCESS_TOKEN"];
                        return Task.CompletedTask;
                    }
                };
            });

            #endregion

            #region metrics

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

            #endregion

            #region api documentation

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #endregion

            builder.Services.AddControllers();
            builder.Services.AddApplication();
            builder.Services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();

            #region exceptions

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddScoped<ICustomExceptionHandler, GenericExceptionHandler>();
            builder.Services.AddScoped<ICustomExceptionHandler, UserExceptionHandler>();

            #endregion

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IUserRepository, UserRepository>();


            var app = builder.Build();

            #region dev tools

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                //app.UseSwagger(options =>
                //{
                //    options.RouteTemplate = "/openapi/{documentName}.json";
                //});
                //app.MapScalarApiReference();
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

            #endregion

            // Configure the HTTP request pipeline.
            app.UseExceptionHandler(_ => { });


            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
