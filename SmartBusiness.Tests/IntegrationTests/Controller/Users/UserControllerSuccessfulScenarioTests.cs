using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SmartBusiness.Contracts.Requests.Users;
using SmartBusiness.Tests.Helpers;

namespace SmartBusiness.Tests.IntegrationTests.Controller.Users
{
    public class UserControllerSuccessfulScenarioTests(CustomWebApplicationFactory factory) 
        : IntegrationTestBase(factory)
    {
        [Fact]
        public async Task Create_WhenSuccessful_ReturnsCreatedResult()
        {
            // Arrange
            const string password = "!Qwerty123";
            var user = IntegrationTestsHelper.GenerateUser();

            var request = new CreateRequest(user.Username, user.Email, password);
            var content = JsonContent.Create(request);

            // Act
            var response = await Client.PostAsync("/api/user", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Update_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var user = IntegrationTestsHelper.GenerateUser();
            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

            var request = new UpdateRequest(user.Id, user.Username, user.Email);
            var content = JsonContent.Create(request);

            // Act
            var response = await Client.PutAsync($"/api/user/{user.Id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_WhenSuccessful_ReturnsNoContent()
        {
            // Arrange
            var user = IntegrationTestsHelper.GenerateUser();
            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

            // Act
            var response = await Client.DeleteAsync($"/api/user/{user.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ChangePassword_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var user = IntegrationTestsHelper.GenerateUser();
            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

            var request = new ChangePasswordRequest(user.Id, "!Qwerty123", "!newPassword123");
            var content = JsonContent.Create(request);

            // Act
            var response = await Client.PutAsync($"/api/user/{user.Id}/change-password", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
