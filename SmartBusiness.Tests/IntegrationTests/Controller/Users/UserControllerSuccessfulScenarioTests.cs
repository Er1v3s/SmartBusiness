using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SmartBusiness.Contracts.Requests.Users;
using SmartBusiness.Tests.ClientBuilder;

namespace SmartBusiness.Tests.IntegrationTests.Controller.Users
{
    public class UserControllerSuccessfulScenarioTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly IIntegrationTestsHelper _integrationTestsHelper;

        public UserControllerSuccessfulScenarioTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _integrationTestsHelper = new IntegrationTestsHelper(factory, _client);
        }

        [Fact]
        public async Task Create_WhenSuccessful_ReturnsCreatedResult()
        {
            // Arrange
            var request = new CreateRequest("JohnDoe", "johndoe@gmail.com", "!Qwerty123");
            var content = JsonContent.Create(request);

            // Act
            var response = await _client.PostAsync("/api/user", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Update_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var (userDto, token) = await _integrationTestsHelper.SeedDatabaseAndGenerateTokenAsync();
            _integrationTestsHelper.SetAuthorizationHeader(token);

            var request = new UpdateRequest(userDto.Id, userDto.Username, userDto.Email);
            var content = JsonContent.Create(request);

            // Act
            var response = await _client.PutAsync($"/api/user/{userDto.Id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_WhenSuccessful_ReturnsNoContent()
        {
            // Arrange
            var (userDto, token) = await _integrationTestsHelper.SeedDatabaseAndGenerateTokenAsync();
            _integrationTestsHelper.SetAuthorizationHeader(token);

            // Act
            var response = await _client.DeleteAsync($"/api/user/{userDto.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ChangePassword_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var (userDto, token) = await _integrationTestsHelper.SeedDatabaseAndGenerateTokenAsync();
            _integrationTestsHelper.SetAuthorizationHeader(token);

            var request = new ChangePasswordRequest(userDto.Id, "!Qwerty123", "!newPassword123");
            var content = JsonContent.Create(request);

            // Act
            var response = await _client.PutAsync($"/api/user/{userDto.Id}/change-password", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
