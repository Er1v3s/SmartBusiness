using MediatR;
using Shared.Exceptions;
using AccountService.Application.Abstracts;

namespace AccountService.Application.Commands.Companies
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

            await _companyRepository.DeleteCompanyAsync(company);
        }
    }
}
