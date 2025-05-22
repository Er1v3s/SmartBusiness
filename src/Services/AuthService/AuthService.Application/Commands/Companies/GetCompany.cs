using FluentValidation;
using MediatR;
using AuthService.Application.Abstracts;
using AuthService.Contracts.DTOs;
using AuthService.Domain.DataTypes;

namespace AuthService.Application.Commands.Companies
{
    public record GetCompanyCommand(Guid UserId, string? Name) : IRequest<List<CompanyDto>>;

    public class GetCompanyCommandValidator : AbstractValidator<GetCompanyCommand>;
    
    public class GetCompanyCommandHandler : IRequestHandler<GetCompanyCommand, List<CompanyDto>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserCompanyRoleRepository _userCompanyRoleRepository;

        public GetCompanyCommandHandler(ICompanyRepository companyRepository, IUserCompanyRoleRepository userCompanyRoleRepository)
        {
            _companyRepository = companyRepository;
            _userCompanyRoleRepository = userCompanyRoleRepository;
        }

        // Get only the companies created by the user
        public async Task<List<CompanyDto>> Handle(GetCompanyCommand request, CancellationToken cancellationToken)
        {
            var query = _companyRepository.GetQueryableIncludingProperties();

            query = query.Where(c => c.UserCompanyRoles.Any(uc => uc.UserId == request.UserId && uc.Role.Name == RoleType.Owner));

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(c => c.Name.Contains(request.Name));

            var companies = await _companyRepository.GetFilteredCompaniesAsync(query, cancellationToken);

            var companiesDto = companies
                .Select(CompanyDto.CreateDto)
                .ToList();

            return companiesDto;
        }
    }
}