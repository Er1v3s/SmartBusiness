using AuthService.Domain.Entities;
using FluentValidation;
using MediatR;

namespace AuthService.Application.Commands.Companies
{
    public record UpdateCompanyCommand(string Id, string Name) : IRequest<Company>;

    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        
    }
    
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Company>
    {
        public Task<Company> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            // TO DO
            throw new NotImplementedException();
        }
    }
}