using MediatR;
using AuthService.Application.Abstracts;
using AuthService.Contracts.DTOs;
using FluentValidation;

namespace AuthService.Application.Commands.CompanyRole
{
    public record GetCompanyRoleCommand(Guid UserId, string? CompanyId, string? Name) : IRequest<List<CompanyDto>>;

    public class GetCompanyCommandValidator : AbstractValidator<GetCompanyRoleCommand>;
    
    public class GetCompanyCommandHandler : IRequestHandler<GetCompanyRoleCommand, List<CompanyDto>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserCompanyRoleRepository _userCompanyRoleRepository;

        public GetCompanyCommandHandler(ICompanyRepository companyRepository, IUserCompanyRoleRepository userCompanyRoleRepository)
        {
            _companyRepository = companyRepository;
            _userCompanyRoleRepository = userCompanyRoleRepository;
        }

        public async Task<List<CompanyDto>> Handle(GetCompanyRoleCommand request, CancellationToken cancellationToken)
        {
            var query = _userCompanyRoleRepository.GetQueryableIncludingProperties();

            // Get only the user company roles created by the user
// TODO            query = query.Where(c => c.User.UserCompanyRoles.Any(uc => uc.UserId == request.UserId));
//
//            if (!string.IsNullOrEmpty(request.CompanyId))
//                query = query.Where(c => c.Company.Id == request.CompanyId);
//
//            if (!string.IsNullOrEmpty(request.Name))
//                query = query.Where(c => c.Name.Contains(request.Name));
//
//            var userCompanyRoles = await _userCompanyRoleRepository.GetFilteredUserCompanyRolesAsync(query, cancellationToken);

            
            var userCompanyRoleDto = new UserCompanyRoleDto
            {
                UserId = ucr.UserId,
                Username = ucr.User.Username
            };
            
            return userCompanyRoleDto;
        }
    }
}