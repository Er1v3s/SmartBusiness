using SmartBusiness.Contracts.DataTransferObjects;

namespace SmartBusiness.Tests.ClientBuilder
{
    public interface IIntegrationTestsHelper : IClassFixture<CustomWebApplicationFactory>
    {
        Task<(UserDto userDto, string token)> SeedDatabaseAndGenerateTokenAsync();
        (UserDto userDto, string token) GenerateUserAndToken();
        void SetAuthorizationHeader(string token);
    }
}
