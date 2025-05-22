using FluentValidation;
using MediatR;
using AuthService.Application.Abstracts;
using AuthService.Contracts.DTOs;
using Shared.Exceptions;
using AuthService.Domain.DataTypes;

namespace AuthService.Application.Commands.Companies
{
    public record GetCompanyByIdCommand(Guid UserId, string CompanyId) : IRequest<CompanyDto>;

    public class GetCompanyByIdCommandValidator : AbstractValidator<GetCompanyCommand>;
    
    public class GetCompanyByIdCommandHandler : IRequestHandler<GetCompanyByIdCommand, CompanyDto>
    {
        private readonly ICompanyRepository _companyRepository;
        
        public GetCompanyByIdCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        // Get only the companies created by the user
        public async Task<CompanyDto> Handle(GetCompanyByIdCommand request, CancellationToken cancellationToken)
        {
            var query = _companyRepository.GetQueryableIncludingProperties();

            query = query.Where(c => c.UserCompanyRoles.Any(uc => uc.UserId == request.UserId && uc.Role.Name == RoleType.Owner));
            query = query.Where(c => c.Id == request.CompanyId);

            var company = await _companyRepository.GetFilteredCompanyAsync(query, cancellationToken)
                          ?? throw new NotFoundException($"Company with id: {request.CompanyId} not found.");

            var companyDto = CompanyDto.CreateDto(company);

            return companyDto;
        }
    }
}