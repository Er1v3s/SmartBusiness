//using System.Net;
//using System.Net.Http.Json;
//using AuthService.Contracts.Requests.Account;
//using AuthService.Contracts.Requests.Auth;
//using AuthService.Tests.Helpers;
//using FluentAssertions;

//namespace AuthService.Tests.IntegrationTests.Controller.Users
//{
//    public class UserControllerSuccessfulScenarioTests(CustomWebApplicationFactory factory) 
//        : IntegrationTestBase(factory)
//    {
//        [Fact]
//        public async Task Create_WhenSuccessful_ReturnsCreatedResult()
//        {
//            // Arrange
//            const string password = "!Qwerty123";
//            var user = IntegrationTestsHelper.GenerateUser();

//            var request = new RegisterUserRequest(user.Username, user.Email, password);
//            var content = JsonContent.Create(request);

//            // Act
//            var response = await Client.PostAsync("/api/auth/register", content);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.Created);
//        }

//        [Fact]
//        public async Task Update_WhenSuccessful_ReturnsOkResult()
//        {
//            // Arrange
//            var user = IntegrationTestsHelper.GenerateUser();
//            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

//            var request = new UpdateUserRequest(user.Username, user.Email);
//            var content = JsonContent.Create(request);

//            // Act
//            var response = await Client.PutAsync($"/api/account/update", content);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//        }

//        [Fact]
//        public async Task Delete_WhenSuccessful_ReturnsNoContent()
//        {
//            // Arrange
//            var user = IntegrationTestsHelper.GenerateUser();
//            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

//            // Act
//            var response = await Client.DeleteAsync($"/api/account/delete");

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
//        }

//        [Fact]
//        public async Task ChangePassword_WhenSuccessful_ReturnsOkResult()
//        {
//            // Arrange
//            var user = IntegrationTestsHelper.GenerateUser();
//            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

//            var request = new ChangePasswordRequest("!Qwerty123", "!newPassword123");
//            var content = JsonContent.Create(request);

//            // Act
//            var response = await Client.PutAsync($"/api/account/change-password", content);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//        }
//    }
//}
