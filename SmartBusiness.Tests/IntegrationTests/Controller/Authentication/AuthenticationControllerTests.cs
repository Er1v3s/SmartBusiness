using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SmartBusiness.Contracts.Requests.Users.Authentication;
using SmartBusiness.Domain.Entities;
using SmartBusiness.Tests.Helpers;

namespace SmartBusiness.Tests.IntegrationTests.Controller.Authentication
{
    public class AuthenticationControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly IIntegrationTestsHelper _integrationTestsHelper;

        public AuthenticationControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _integrationTestsHelper = new IntegrationTestsHelper(factory, _client);
        }
        
        [Fact]
        public async Task Login_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var user = await _integrationTestsHelper.SeedDatabaseAndGenerateUserAsync();

            var request = new LoginRequest(user.Email, user.PasswordHash); // Password is not hashed yet.
            var content = JsonContent.Create(request); 
            
            // Act 
            var result = await _client.PostAsync($"/api/user/authentication", content);
            
            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}