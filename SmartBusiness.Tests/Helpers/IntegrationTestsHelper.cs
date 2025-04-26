using SmartBusiness.Contracts.DataTransferObjects;
using SmartBusiness.Domain.Entities;
using SmartBusiness.Infrastructure;
using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartBusiness.Application.Abstracts;

namespace SmartBusiness.Tests.Helpers
{
    public class IntegrationTestsHelper : IIntegrationTestsHelper
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly IMapper? _mapper;
        private readonly IAuthTokenProcessor _authTokenProcessor;
        private readonly HttpClient _client;

        public IntegrationTestsHelper(CustomWebApplicationFactory factory, HttpClient httpClient)
        {
            _factory = factory;
            _mapper = factory.Mapper;
            _authTokenProcessor = factory.AuthTokenProcessor;
            _client = httpClient;
        }

        public async Task<(UserDto userDto, string token)> SeedDatabaseAndGenerateTokenAsync()
        {
            var user = new User
            {
                Username = "johndoe",
                Email = "johndoe@gmail.com",
                PasswordHash = "!Qwerty123",
            };

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SmartBusinessDbContext>();
            _factory.SeedInMemoryDatabase(db, user);

            var userFromDb = await db.Users.FirstAsync(u => u.Email == user.Email);
            var userDto = _mapper.Map<UserDto>(userFromDb);
            var token = _authTokenProcessor.GenerateJwtToken(userDto).jwtToken;

            return (userDto, token);
        }

        public (UserDto userDto, string token) GenerateUserAndToken()
        {
            var user = new User
            {
                Username = "johndoe",
                Email = "johndoe@gmail.com",
                PasswordHash = "!Qwerty123",
            };

            using var scope = _factory.Services.CreateScope();
            var userDto = _mapper.Map<UserDto>(user);
            return (userDto, _authTokenProcessor.GenerateJwtToken(userDto).jwtToken);
        }

        public void SetAuthorizationHeader(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
