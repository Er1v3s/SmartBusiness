using Microsoft.Extensions.DependencyInjection;
using SmartBusiness.Infrastructure;
using SmartBusiness.Tests.Helpers;

namespace SmartBusiness.Tests.IntegrationTests
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
            var dbContext = _scope.ServiceProvider.GetRequiredService<SmartBusinessDbContext>();
            IntegrationTestsHelper = new IntegrationTestsHelper(dbContext);
        }

        public void Dispose()
        {
            _scope.Dispose();
            Client.Dispose();
        }
    }
}
