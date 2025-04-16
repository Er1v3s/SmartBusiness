using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartBusiness.Application.Services;
using SmartBusiness.Contracts.DataTransferObjects;
using SmartBusiness.Contracts.Errors;
using SmartBusiness.Domain.Entities;
using SmartBusiness.Infrastructure;

namespace SmartBusiness.Application.Commands.Authentication.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly SmartBusinessDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public LoginUserCommandHandler(SmartBusinessDbContext dbContext, IPasswordHasher<User> passwordHasher, IMapper mapper, IAuthenticationService authenticationService)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null)
                throw new NotFoundException("User not found");

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
                throw new InvalidPasswordException("Incorrect password");

            var userDto = _mapper.Map<UserDto>(user);

            var token = _authenticationService.GenerateAuthenticationToken(userDto);

            return token;
        }
    }
}
