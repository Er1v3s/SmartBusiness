using MediatR;
using AccountService.Application.Abstracts;
using AccountService.Application.Commands.Abstracts;
using AccountService.Contracts.DTOs;
using Shared.Exceptions;

namespace AccountService.Application.Commands.CompanyRole
{
    public record UpdateCompanyRoleCommand(string CompanyId, string RoleId, string Name) : UserCompanyRoleCommand(Name), 
        IRequest<UserCompanyRoleDto>;

    public class UpdateCompanyRoleCommandValidator : UserCompanyRoleCommandValidator<UpdateCompanyRoleCommand>;

    public class UpdateCompanyRoleCommandHandler : IRequestHandler<UpdateCompanyRoleCommand, UserCompanyRoleDto>
    {
        private readonly IUserCompanyRoleRepository _userCompanyRoleRepository;

        public UpdateCompanyRoleCommandHandler(IUserCompanyRoleRepository userCompanyRoleRepository)
        {
            _userCompanyRoleRepository = userCompanyRoleRepository;
        }

        public async Task<UserCompanyRoleDto> Handle(UpdateCompanyRoleCommand request, CancellationToken cancellationToken)
        {
            var query = _userCompanyRoleRepository.GetQueryableIncludingProperties();
            query = query.Where(ucr => ucr.CompanyId == request.CompanyId);
            query = query.Where(ucr => ucr.Role.Id == request.RoleId);

            var userCompanyRole = await _userCompanyRoleRepository.GetFilteredUserCompanyRoleAsync(query, cancellationToken)
                       ?? throw new NotFoundException("Role not found");

            await _userCompanyRoleRepository.UpdateUserCompanyRoleAsync(userCompanyRole);

            var userCompanyRoleDto = UserCompanyRoleDto.CreateDto(userCompanyRole);

            return userCompanyRoleDto;
        }
    }
}