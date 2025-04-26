using SmartBusiness.Contracts.DataTransferObjects;
using SmartBusiness.Domain.Entities;
using SmartBusiness.Infrastructure;
using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SmartBusiness.Application.Abstracts;

namespace SmartBusiness.Tests.Helpers
{
    public class IntegrationTestsHelper : IIntegrationTestsHelper
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly IMapper? _mapper;
        private readonly IAuthTokenProcessor _authTokenProcessor;
        private readonly HttpClient _client;
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

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

            await SeedInMemoryDatabaseAsync(user);

            var userFromDb = await  GetUserFromDbAsync(user);
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

        public async Task<User> SeedDatabaseAndGenerateUserAsync()
        {
            var user = new User
            {
                Username = "johndoe",
                Email = "johndoe@gmail.com",
                PasswordHash = "!Qwerty123",
            };

            // deep copy of object 
            var json = JsonConvert.SerializeObject(user);
            
            await SeedInMemoryDatabaseAsync(user);

            var deserializedUser = JsonConvert.DeserializeObject<User>(json);
            return deserializedUser!;
        }

        public void SetAuthorizationHeader(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task EnsureThereIsNoDataInDb()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SmartBusinessDbContext>();
            var isDbNotEmpty = await db.Users.AnyAsync();
            if (isDbNotEmpty)
            {
                var users = await db.Users.ToListAsync();
                db.Users.RemoveRange(users);
                await db.SaveChangesAsync();
            }
        }
        
        private async Task SeedInMemoryDatabaseAsync(User user)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SmartBusinessDbContext>();
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }

        private async Task<User> GetUserFromDbAsync(User user)
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SmartBusinessDbContext>();
            var userFromDb = await db.Users.FirstAsync(u => u.Email == user.Email);
            
            return userFromDb;
        }
    }
}
