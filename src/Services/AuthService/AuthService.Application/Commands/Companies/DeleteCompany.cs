using AuthService.Domain.Entities;
using MediatR;

namespace AuthService.Application.Commands.Companies
{
    public record DeleteCompanyCommand(string Id) : IRequest<Company>;

    public class DeleteCompanyCommandHandler() : IRequestHandler<DeleteCompanyCommand, Company>
    {
        public Task<Company> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
