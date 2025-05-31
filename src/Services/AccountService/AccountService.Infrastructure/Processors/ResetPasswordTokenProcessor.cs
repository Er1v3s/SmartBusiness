using AccountService.Application.Abstracts;
using System.Security.Cryptography;

namespace AccountService.Infrastructure.Processors
{
    public class ResetPasswordTokenProcessor : IResetPasswordTokenProcessor
    {
        public string GenerateResetPasswordToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var token = Convert.ToBase64String(randomNumber);
            token = token.Replace("+", "").Replace("/", "").Replace("=", "");

            return token;
        }
    }
}
