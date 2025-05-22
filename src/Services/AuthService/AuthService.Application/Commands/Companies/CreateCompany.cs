using MediatR;
using Shared.Exceptions;
using AuthService.Domain.Entities;
using AuthService.Application.Abstracts;
using AuthService.Application.Commands.Abstracts;
using AuthService.Contracts.Exceptions.Auth;
using AuthService.Domain.DataTypes;
using AuthService.Contracts.DTOs;

namespace AuthService.Application.Commands.Companies
{
    public record CreateCompanyCommand(Guid UserId, string Name) : CompanyCommand(Name), IRequest<CompanyDto>;

    public class CreateCompanyCommandValidator : CompanyCommandValidator<CreateCompanyCommand>;
    
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        
        public CreateCompanyCommandHandler(ICompanyRepository companyRepository, IUserRepository userRepository)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }
    
        public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            // Check if the user exists to avoid creating a company without a user
            // This is important because JWT might be still valid but the user might not exist anymore
            var query = _userRepository.GetQueryableIncludingProperties();
            query = query.Where(u => u.Id == request.UserId);

            // Check if the user exists
            var user = await _userRepository.GetFilteredUserAsync(query, cancellationToken)
                ?? throw new AuthenticationException("User not found");

            // Check if the user already has a company with the same name
            if(user.UserCompanyRoles.Any(uc => uc.Company.Name == request.Name))
                throw new ConflictException("User already has a company with the same name");

            var company = new Company(request.Name);
            var role = new Role(RoleType.Owner);
            var ucr = new UserCompanyRole(user.Id, company.Id, role.Id)
            {
                User = user,
                Company = company,
                Role = role,
            };

            company.UserCompanyRoles.Add(ucr);

            await _companyRepository.AddCompanyAsync(company);

            var companyDto = CompanyDto.CreateDto(company);
            
            return companyDto;
        }
    }
}