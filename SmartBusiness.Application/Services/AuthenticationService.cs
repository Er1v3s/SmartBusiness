using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using SmartBusiness.Contracts.DataTransferObjects;

namespace SmartBusiness.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationSettings _authenticationSettings;

        public AuthenticationService(AuthenticationSettings authenticationSettings)
        {
            _authenticationSettings = authenticationSettings;
        }

        public string GenerateAuthenticationToken(UserDto userDto)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new (ClaimTypes.Name, $"{userDto.Username}"),
                new (ClaimTypes.Email, userDto.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                issuer: _authenticationSettings.JwtIssuer,
                audience: _authenticationSettings.JwtIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
