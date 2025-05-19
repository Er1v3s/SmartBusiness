using FluentValidation;
using MediatR;
using FluentValidation;
using AuthService.Application.Abstracts;
using AuthService.Contracts.DTOs;

namespace AuthService.Application.Commands.Companies
{
    public record GetCompanyCommand(Guid UserId, string? Id, string? Name) : IRequest<List<CompanyDto>>;

    public class GetCompanyCommandValidator : AbstractValidator<GetCompanyCommand>;
    
    public class GetCompanyCommandHandler : IRequestHandler<GetCompanyCommand, List<CompanyDto>>
    {
        private readonly ICompanyRepository _companyRepository;
        
        public GetCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
    }
    
        public async Task<List<CompanyDto>> Handle(GetCompanyCommand request, CancellationToken cancellationToken)
    {
            var query = _companyRepository.GetQueryableIncludingProperties();

            // Get only the companies created by the user
            query = query.Where(c => c.UserCompanyRoles.Any(uc => uc.UserId == request.UserId));

            if (!string.IsNullOrEmpty(request.Id))
                query = query.Where(c => c.Id == request.Id);

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