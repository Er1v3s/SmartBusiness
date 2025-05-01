using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AuthService.Domain.Entities;
using AuthService.Application.Abstracts;
using AuthService.Contracts.Exceptions.Users;
using AuthService.Contracts.DataTransferObjects;

namespace AuthService.Application.Commands.Users.Authentication.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IAuthTokenProcessor _authTokenProcessor;

        public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IMapper mapper, IAuthTokenProcessor authTokenProcessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _authTokenProcessor = authTokenProcessor;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

            if(user == null)
                throw new UserNotFoundException();

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
                throw new InvalidPasswordException("Incorrect password");

            var userDto = _mapper.Map<UserDto>(user);

            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(userDto);

            // Here you can set the expiration date in the response headers if needed

            return jwtToken;
        }
    }
}
