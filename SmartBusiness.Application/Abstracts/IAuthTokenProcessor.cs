using SmartBusiness.Contracts.DataTransferObjects;

namespace SmartBusiness.Application.Abstracts
{
    public interface IAuthTokenProcessor
    {
        (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(UserDto userDto);
        string GenerateRefreshToken();
        void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
    }
}

