using MediatR;
using FluentValidation;
using AuthService.Contracts.DTOs;
using AuthService.Application.Abstracts;

namespace AuthService.Application.Commands.CompanyRole
{
    public record GetCompanyRolesCommand(Guid UserId, string? CompanyId, string? Name) : IRequest<List<UserCompanyRoleDto>>;

    public class GetCompanyRolesCommandValidator : AbstractValidator<GetCompanyRolesCommand>;

    public class GetCompanyRolesCommandHandler : IRequestHandler<GetCompanyRolesCommand, List<UserCompanyRoleDto>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserCompanyRoleRepository _userCompanyRoleRepository;

        public GetCompanyRolesCommandHandler(ICompanyRepository companyRepository, IUserCompanyRoleRepository userCompanyRoleRepository)
        {
            _companyRepository = companyRepository;
            _userCompanyRoleRepository = userCompanyRoleRepository;
        }

        public async Task<List<UserCompanyRoleDto>> Handle(GetCompanyRolesCommand request, CancellationToken cancellationToken)
        {
            var query = _userCompanyRoleRepository.GetQueryableIncludingProperties();

            if (!string.IsNullOrEmpty(request.CompanyId))
                query = query.Where(ucr => ucr.CompanyId == request.CompanyId);

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(ucr => ucr.Role.Name.Contains(request.Name));

            var userCompanyRoles = await _userCompanyRoleRepository.GetFilteredUserCompanyRolesAsync(query, cancellationToken);

            var userCompanyRoleDto = userCompanyRoles
                .Select(UserCompanyRoleDto.CreateDto)
                .ToList();

            return userCompanyRoleDto;
        }
    }
}