using MediatR;
using AuthService.Application.Abstracts;
using Shared.Exceptions;

namespace AuthService.Application.Commands.CompanyRole
{
    public record DeleteCompanyRoleCommand(string CompanyId, string RoleId) : IRequest;

    public class DeleteCompanyRoleCommandHandler : IRequestHandler<DeleteCompanyRoleCommand>
    {
        private readonly IUserCompanyRoleRepository _userCompanyRoleRepository;

        public DeleteCompanyRoleCommandHandler(IUserCompanyRoleRepository userCompanyRoleRepository)
        {
            _userCompanyRoleRepository = userCompanyRoleRepository;
        }

        public async Task Handle(DeleteCompanyRoleCommand request, CancellationToken cancellationToken)
        {
            var query = _userCompanyRoleRepository.GetQueryableIncludingProperties();
            query = query.Where(ucr => ucr.CompanyId == request.CompanyId);
            query = query.Where(ucr => ucr.RoleId == request.RoleId);

            var userCompanyRole = await _userCompanyRoleRepository.GetFilteredUserCompanyRoleAsync(query, cancellationToken)
                                  ?? throw new NotFoundException("Role not found");

            await _userCompanyRoleRepository.RemoveUserCompanyRoleAsync(userCompanyRole);
        }
    }
}
