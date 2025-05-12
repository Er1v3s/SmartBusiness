using AuthService.Domain.Entities;
using FluentValidation;
using MediatR;

namespace AuthService.Application.Commands.Companies
{
    public record CreateCompanyCommand(string? Name) : IRequest<Company> { }

    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        
    }
    
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Company>
    {
        public Task<Company> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}