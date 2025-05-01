using AuthService.Infrastructure;
using AuthService.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>, IDisposable
    {
        private readonly IServiceScope _scope;
        protected readonly HttpClient Client;
        protected readonly IIntegrationTestsHelper IntegrationTestsHelper;
        
        protected IntegrationTestBase(CustomWebApplicationFactory factory)
        {
            _scope = factory.Services.CreateScope();
            Client = factory.CreateClient();
            var dbContext = _scope.ServiceProvider.GetRequiredService<AuthServiceDbContext>();
            IntegrationTestsHelper = new IntegrationTestsHelper(dbContext);
        }

        public void Dispose()
        {
            _scope.Dispose();
            Client.Dispose();
        }
    }
}
