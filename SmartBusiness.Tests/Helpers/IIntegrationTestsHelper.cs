using SmartBusiness.Domain.Entities;

namespace SmartBusiness.Tests.Helpers
{
    public interface IIntegrationTestsHelper : IClassFixture<CustomWebApplicationFactory>
    {
        Task SeedInMemoryDatabaseAsync(User user);
        User GenerateUser();
    }
}
