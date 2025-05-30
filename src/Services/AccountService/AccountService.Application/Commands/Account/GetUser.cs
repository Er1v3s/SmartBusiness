using MediatR;
using FluentValidation;
using AccountService.Application.Abstracts;
using AccountService.Contracts.DTOs;
using AccountService.Contracts.Exceptions.Users;
using AutoMapper;

namespace AccountService.Application.Commands.Account
{
    public record GetUserCommand(Guid Id) : IRequest<UserDto>;

    public class GetUserCommandValidator : AbstractValidator<GetUserCommand>;

    public class GetUserCommandHandler : IRequestHandler<GetUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id)
                       ?? throw new UserNotFoundException();

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }
    }
}