using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SmartBusiness.Contracts.Requests.Users;
using SmartBusiness.Tests.Helpers;

namespace SmartBusiness.Tests.IntegrationTests.Controller.Users
{
    public class UserControllerExceptionScenarioTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly IIntegrationTestsHelper _integrationTestsHelper;

        public UserControllerExceptionScenarioTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _integrationTestsHelper = new IntegrationTestsHelper(factory, _client);
        }

        [Fact]
        public async Task Create_WhenUserWithThisEmailExist_ReturnsBadRequest()
        {
            // Arrange
            var (userDto, token) = await _integrationTestsHelper.SeedDatabaseAndGenerateTokenAsync();
            _integrationTestsHelper.SetAuthorizationHeader(token);

            var request = new UpdateRequest(Guid.NewGuid(), "johndoe", userDto.Email);
            var content = JsonContent.Create(request);

            // Act
            var response = await _client.PostAsync("/api/user", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var (userDto, token) = _integrationTestsHelper.GenerateUserAndToken();
            _integrationTestsHelper.SetAuthorizationHeader(token);


            var request = new UpdateRequest(userDto.Id, userDto.Username, userDto.Email);
            var content = JsonContent.Create(request);

            // Act
            var response = await _client.PutAsync($"/api/user/{userDto.Id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ChangePassword_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            const string password = "!Qwerty123";
            var (userDto, token) = _integrationTestsHelper.GenerateUserAndToken();
            _integrationTestsHelper.SetAuthorizationHeader(token);

            // Arrange
            var request = new ChangePasswordRequest(userDto.Id, password, "!newPassword123");
            var content = JsonContent.Create(request);

            // Act
            var response = await _client.PutAsync($"/api/user/{userDto.Id}/change-password", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ChangePassword_WhenUserPassWrongOldPassword_ReturnsConflict()
        {
            // Arrange
            const string password = "!Qwerty123";
            var (userDto, token) = await _integrationTestsHelper.SeedDatabaseAndGenerateTokenAsync();
            _integrationTestsHelper.SetAuthorizationHeader(token);

            var request = new ChangePasswordRequest(userDto.Id, $"{password}-wrongPassword", $"{password}-newPassword");
            var content = JsonContent.Create(request);

            // Act
            var response = await _client.PutAsync($"/api/user/{userDto.Id}/change-password", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_WhenUserPassTheSameOldAndNewPassword_ThrowsInvalidPasswordException()
        {
            // Arrange
            var (userDto, token) = await _integrationTestsHelper.SeedDatabaseAndGenerateTokenAsync();
            _integrationTestsHelper.SetAuthorizationHeader(token);

            var request = new ChangePasswordRequest(userDto.Id, "!Qwerty123", "!Qwerty123");
            var content = JsonContent.Create(request);

            // Act
            var response = await _client.PutAsync($"/api/user/{userDto.Id}/change-password", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_WhenUserDoesNotExist_ThrowsUserNotFoundException()
        {
            // Arrange
            var (userDto, token) = _integrationTestsHelper.GenerateUserAndToken();
            _integrationTestsHelper.SetAuthorizationHeader(token);

            // Act
            var response = await _client.DeleteAsync($"/api/user/{userDto.Id}", CancellationToken.None);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        
    }
}