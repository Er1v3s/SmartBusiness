using AuthService.Domain.Entities;
using FluentValidation;
using MediatR;

namespace AuthService.Application.Commands.Companies
{
    public record GetCompanyCommand(string? Id, string? Name) : IRequest<List<Company>>;

    public class GetCompanyCommandValidator : AbstractValidator<GetCompanyCommand>
    {
        
    }
    
    public class GetCompanyCommandHandler : IRequestHandler<GetCompanyCommand, List<Company>>
    {
        public Task<List<Company>> Handle(GetCompanyCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}