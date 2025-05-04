using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuthService.Api.Handlers;
using AuthService.Api.Handlers.CustomExceptionHandlers;
using AuthService.Application;
using AuthService.Application.Abstracts;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Options;
using AuthService.Infrastructure.Processors;
using AuthService.Infrastructure.Repositories;


namespace AuthService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region database connection

            builder.Services.AddDbContext<AuthServiceDbContext>(options =>
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

            #region exceptions handling

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddScoped<ICustomExceptionHandler, GenericExceptionHandler>();
            builder.Services.AddScoped<ICustomExceptionHandler, UserExceptionHandler>();

            #endregion 

            builder.Services.AddApplication();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            #region api documentation

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SmartBusiness.AuthService API",
                    Version = "v1",
                    Description = "SmartBusiness.AuthService API",
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

            builder.Services.AddControllers();

            var app = builder.Build();

            #region dev tools

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("http://localhost:2100/swagger/v1/swagger.json", "SmartBusiness.AuthService API");
                    c.RoutePrefix = "swagger";
                });

                try
                {
                    using var serviceScope = app.Services.CreateScope();
                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<AuthServiceDbContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception)
                {
                    Console.WriteLine("info: Cannot execute migrations. Database is already up to date");
                }
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
