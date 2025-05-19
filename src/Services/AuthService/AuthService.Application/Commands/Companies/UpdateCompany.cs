using FluentValidation;
using MediatR;
using Shared.Exceptions;
using AuthService.Application.Abstracts;
using AuthService.Application.Commands.Abstracts;
using AuthService.Contracts.Exceptions.Auth;
using AuthService.Contracts.DTOs;

namespace AuthService.Application.Commands.Companies
{
    public record UpdateCompanyCommand(Guid UserId, string Id, string Name) : CompanyCommand(Name), IRequest<CompanyDto>;

    public class UpdateCompanyCommandValidator : CompanyCommandValidator<UpdateCompanyCommand>;
    
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, CompanyDto>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;

        public UpdateCompanyCommandHandler(ICompanyRepository companyRepository, IUserRepository userRepository)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }

        // If the user is not the owner of the company, he should not be able to update it
        public async Task<CompanyDto> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(request.Id)
                ?? throw new NotFoundException($"Company with ID {request.Id} not found.");

            // Check if the user exists to avoid creating a company without a user
            // This is important because JWT might be still valid but the user might not exist anymore
            var query = _userRepository.GetQueryableIncludingProperties();
            query = query.Where(u => u.Id == request.UserId);

            // Check if the user exists
            var user = await _userRepository.GetFilteredUserAsync(query, cancellationToken)
                       ?? throw new AuthenticationException("User not found");

            // Check if the user already has a company with the same name
            if (user.UserCompanyRoles.Any(uc => uc.Company.Name == request.Name))
                throw new ConflictException("User already has a company with the same name");

            // Check if the user is the owner of the company
            // Controller decorator check this, but user might have "Owner" claim of the other company
            var isUserOwnerOfCompany = await _companyRepository.IsUserOwnerOfCompanyAsync(user.Id, company.Id);
            if (!isUserOwnerOfCompany)
                throw new ForbiddenException("User should be owner of the company");

            company.Name = request.Name;
            await _companyRepository.UpdateCompanyAsync(company);

            var companyDto = CompanyDto.CreateDto(company);

            return companyDto;
        }
    }
}