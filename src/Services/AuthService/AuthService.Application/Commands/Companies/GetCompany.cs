using FluentValidation;
using MediatR;

namespace AuthService.Application.Commands.Companies
{
    public record GetCompanyCommand(string? Id, string? Name) : IRequest<List<Company>>;

    public class GetCompanyCommandValidator : AbstractValidator<GetCompanyCommand>
    {
        
    }
    
    public class GetCompanyCommandHandler : IRequestHandler<GetCompany, Company>
    {
        public Task<string> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            // TO DO
        }
    }
}