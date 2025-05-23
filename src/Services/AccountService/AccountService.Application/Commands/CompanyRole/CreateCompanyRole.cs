using MediatR;
using AccountService.Application.Abstracts;
using AccountService.Application.Commands.Abstracts;
using AccountService.Contracts.DTOs;
using AccountService.Contracts.Exceptions.Users;
using AccountService.Domain.Entities;
using Shared.Exceptions;

namespace AccountService.Application.Commands.CompanyRole
{
    public record CreateCompanyRoleCommand(Guid UserId, string CompanyId, string Name) : UserCompanyRoleCommand(Name),
        IRequest<UserCompanyRoleDto>;

    public class CreateCompanyRoleCommandValidator : UserCompanyRoleCommandValidator<CreateCompanyRoleCommand>;

    public class CreateCompanyRoleCommandHandler : IRequestHandler<CreateCompanyRoleCommand, UserCompanyRoleDto>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserCompanyRoleRepository _userCompanyRoleRepository;

        public CreateCompanyRoleCommandHandler(
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IUserCompanyRoleRepository userCompanyRoleRepository
            )
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _userCompanyRoleRepository = userCompanyRoleRepository;
        }

        public async Task<UserCompanyRoleDto> Handle(CreateCompanyRoleCommand request, CancellationToken cancellationToken)
        {
            // Check if the user exists
            var user = await _userRepository.GetUserByIdAsync(request.UserId)
                           ?? throw new UserNotFoundException("User not found");

            // Check if the company exists
            var company = await _companyRepository.GetCompanyByIdAsync(request.CompanyId)
                          ?? throw new NotFoundException("Company not found");

            var role = new Role(request.Name);
            var userCompanyRole = new UserCompanyRole(user.Id, company.Id, role.Id)
            {
                User = user,
                Company = company,
                Role = role
            };

            await _userCompanyRoleRepository.AddUserCompanyRoleAsync(userCompanyRole);

            var userCompanyRoleDto = UserCompanyRoleDto.CreateDto(userCompanyRole);
            
            return userCompanyRoleDto;
        }
    }
}