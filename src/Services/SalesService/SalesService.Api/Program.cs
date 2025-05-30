using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SalesService.Api.Handlers;
using SalesService.Api.Handlers.CustomExceptionHandlers;
using SalesService.Application;
using SalesService.Infrastructure;

namespace SalesService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region database connection

            builder.Services.AddDbContext<SalesDbContext>(options =>
            {
                if (builder.Environment.EnvironmentName != "Testing")
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString"));
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

            #region exceptions handling

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddScoped<ICustomExceptionHandler, GenericExceptionHandler>();
            builder.Services.AddScoped<ICustomExceptionHandler, ProductExceptionHandler>();

            #endregion

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers();

            #region api documentation

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SmartBusiness.SalesService API",
                    Version = "v1",
                    Description = "SmartBusiness.SalesService API",
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
                    c.SwaggerEndpoint("http://localhost:2200/swagger/v1/swagger.json", "SmartBusiness.SalesService API");
                    c.RoutePrefix = "swagger";
                });

                //try
                //{
                //    using var serviceScope = app.Services.CreateScope();
                //    var dbContext = serviceScope.ServiceProvider.GetRequiredService<SalesDbContext>();
                //    dbContext.Database.Migrate();
                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("info: Cannot execute migrations. Database is already up to date");
                //}
            }

            #endregion

            // DO NOT CHANGE ORDER !!!
            app.UseExceptionHandler(_ => { });

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
