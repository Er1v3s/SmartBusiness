using SmartBusiness.Contracts.DataTransferObjects;
using SmartBusiness.Domain.Entities;

namespace SmartBusiness.Tests.Helpers
{
    public interface IIntegrationTestsHelper : IClassFixture<CustomWebApplicationFactory>
    {
        Task<(UserDto userDto, string token)> SeedDatabaseAndGenerateTokenAsync();
        (UserDto userDto, string token) GenerateUserAndToken();
        Task<User> SeedDatabaseAndGenerateUserAsync();
        void SetAuthorizationHeader(string token);
        Task EnsureThereIsNoDataInDb();
    }
}
