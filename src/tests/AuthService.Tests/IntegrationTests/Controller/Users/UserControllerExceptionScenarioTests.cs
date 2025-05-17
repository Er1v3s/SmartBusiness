//using System.Net;
//using System.Net.Http.Json;
//using AuthService.Contracts.Requests.Account;
//using AuthService.Contracts.Requests.Auth;
//using AuthService.Tests.Helpers;
//using FluentAssertions;

//namespace AuthService.Tests.IntegrationTests.Controller.Users
//{
//    public class UserControllerExceptionScenarioTests(CustomWebApplicationFactory factory)
//        : IntegrationTestBase(factory)
//    {
//        [Fact]
//        public async Task Create_WhenUserWithThisEmailExist_ReturnsBadRequest()
//        {
//            // Arrange
//            var user = IntegrationTestsHelper.GenerateUser();

//            // seed the database to force exception
//            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

//            var request = new RegisterUserRequest(user.Username, user.Email, user.Email);
//            var content = JsonContent.Create(request);

//            // Act
//            var response = await Client.PostAsync("/api/auth/register", content);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task Update_WhenUserDoesNotExist_ReturnsNotFound()
//        {
//            // Arrange
//            var user = IntegrationTestsHelper.GenerateUser();

//            var request = new UpdateUserRequest(user.Username, user.Email);
//            var content = JsonContent.Create(request);

//            // Act
//            var response = await Client.PutAsync($"/api/account/update", content);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//        }

//        [Fact]
//        public async Task ChangePassword_WhenUserDoesNotExist_ReturnsNotFound()
//        {
//            // Arrange
//            const string password = "!Qwerty123";

//            var user = IntegrationTestsHelper.GenerateUser();

//            // Arrange
//            var request = new ChangePasswordRequest(password, "!newPassword123");
//            var content = JsonContent.Create(request);

//            // Act
//            var response = await Client.PutAsync($"/api/account/change-password", content);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//        }

//        [Fact]
//        public async Task ChangePassword_WhenUserPassWrongOldPassword_ReturnsConflict()
//        {
//            // Arrange
//            const string password = "!Qwerty123";

//            var user = IntegrationTestsHelper.GenerateUser();

//            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

//            var request = new ChangePasswordRequest($"{password}-wrongPassword", $"{password}-newPassword");
//            var content = JsonContent.Create(request);

//            // Act
//            var response = await Client.PutAsync($"/api/account/change-password", content);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task ChangePassword_WhenUserPassTheSameOldAndNewPassword_ThrowsInvalidPasswordException()
//        {
//            // Arrange
//            var user = IntegrationTestsHelper.GenerateUser();

//            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

//            var request = new ChangePasswordRequest("!Qwerty123", "!Qwerty123");
//            var content = JsonContent.Create(request);

//            // Act
//            var response = await Client.PutAsync($"/api/account/change-password", content);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task Delete_WhenUserDoesNotExist_ThrowsUserNotFoundException()
//        {
//            // Arrange
//            var user = IntegrationTestsHelper.GenerateUser();

//            // Act
//            var response = await Client.DeleteAsync($"/api/account/delete", CancellationToken.None);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//        }
//    }
//}