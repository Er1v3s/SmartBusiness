using MediatR;
using Shared.Exceptions;
using AuthService.Application.Abstracts;
using AuthService.Contracts.Exceptions.Auth;

namespace AuthService.Application.Commands.Companies
{
    public record DeleteCompanyCommand(Guid UserId, string CompanyId) : IRequest;

    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        public DeleteCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        // If the user is not the creator of the company, he should not be able to delete it
        public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(request.CompanyId)
                ?? throw new NotFoundException($"Company with ID {request.CompanyId} not found.");

            // Check if the user is the owner of the company
            // Controller decorator check this, but user might have "Owner" claim of other company
            var isUserOwnerOfCompany = await _companyRepository.IsUserOwnerOfCompanyAsync(request.UserId, company.Id);
            if (!isUserOwnerOfCompany)
                throw new ForbiddenException("User should be owner of the company");

            await _companyRepository.DeleteCompanyAsync(company);
        }
    }
}
