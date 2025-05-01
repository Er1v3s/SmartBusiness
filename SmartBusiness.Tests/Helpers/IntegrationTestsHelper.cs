using AuthService.Domain.Entities;
using AuthService.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Tests.Helpers
{
    public class IntegrationTestsHelper(AuthServiceDbContext dbContext) : IIntegrationTestsHelper
    {
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public User GenerateUser()
        {
            // a unique identifier is needed to avoid conflicts in the inMemoryDatabase
            var uniqueId = Guid.NewGuid().ToString().Substring(0, 8);

            return new User
            {
                Username = $"user-{uniqueId}",
                Email = $"user-{uniqueId}@gmail.com",
                PasswordHash = "!Qwerty123",
            };
        }

        public async Task SeedInMemoryDatabaseAsync(User user)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
