using MediatR;
using AuthService.Application.Abstracts;

namespace AuthService.Application.Commands.CompanyRole
{
    public record DeleteCompanyRoleCommand(Guid UserId, string CompanyId) : IRequest;

    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyRoleCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        public DeleteCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        
        public async Task Handle(DeleteCompanyRoleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
