using FluentValidation;
using MediatR;

namespace AuthService.Application.Commands.Companies
{
    public record CreateCompanyCommand(string? Id, string? Name) : IRequest<Company>;

    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        
    }
    
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Company>
    {
        public Task<string> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            
        }
    }
}