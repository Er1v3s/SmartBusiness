using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartBusiness.Api;
using SmartBusiness.Application.Abstracts;
using SmartBusiness.Domain.Entities;
using SmartBusiness.Infrastructure;

namespace SmartBusiness.Tests.Helpers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public IAuthTokenProcessor? AuthTokenProcessor { get; private set; }
        public IMapper? Mapper { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddDbContext<SmartBusinessDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SmartBusinessDbContext>();
                db.Database.EnsureCreated();

                // Get instance of IAuthTokenProcessor from DI
                AuthTokenProcessor = scope.ServiceProvider.GetRequiredService<IAuthTokenProcessor>();
                Mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            });
        }
    }
}