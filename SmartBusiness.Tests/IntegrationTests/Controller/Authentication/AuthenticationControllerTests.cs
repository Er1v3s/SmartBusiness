using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SmartBusiness.Contracts.Requests.Users.Authentication;
using SmartBusiness.Tests.Helpers;

namespace SmartBusiness.Tests.IntegrationTests.Controller.Authentication
{
    public class AuthenticationControllerTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
    {
        [Fact]
        public async Task Login_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            const string password = "!Qwerty123";
            var user = IntegrationTestsHelper.GenerateUser();
            // seed the database to force exception
            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

            // user.PasswordHash is hashed because of the seed db method.
            var request = new LoginRequest(user.Email, password);
            var content = JsonContent.Create(request); 
            
            // Act 
            var result = await Client.PostAsync($"/api/auth/login", content);
            
            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task Login_WhenUserDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            const string password = "!Qwerty123";
            var user = IntegrationTestsHelper.GenerateUser();

            // user.PasswordHash is hashed because of the seed db method.
            var request = new LoginRequest(user.Email, password);
            var content = JsonContent.Create(request); 
            
            // Act 
            var result = await Client.PostAsync($"/api/auth/login", content);
            
            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task Login_WhenInvalidPassword_ReturnsBadRequestResult()
        {
            // Arrange
            var user = IntegrationTestsHelper.GenerateUser();
            await IntegrationTestsHelper.SeedInMemoryDatabaseAsync(user);

            var request = new LoginRequest(user.Email, "!invalidPassword123");
            var content = JsonContent.Create(request); 
            
            // Act 
            var result = await Client.PostAsync($"/api/auth/login", content);
            
            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}