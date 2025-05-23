using MediatR;
using Shared.Exceptions;
using AccountService.Application.Abstracts;
using AccountService.Application.Commands.Abstracts;
using AccountService.Contracts.Exceptions.Auth;
using AccountService.Contracts.DTOs;

namespace AccountService.Application.Commands.Companies
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

            company.Name = request.Name;
            await _companyRepository.UpdateCompanyAsync(company);

            var companyDto = CompanyDto.CreateDto(company);

            return companyDto;
        }
    }
}