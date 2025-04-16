using SmartBusiness.Contracts.DataTransferObjects;

namespace SmartBusiness.Application.Services;

public interface IAuthenticationService
{
    string GenerateAuthenticationToken(UserDto userDto);
}