using MediatR;
using FluentValidation;
using AccountService.Contracts.DTOs;
using AccountService.Application.Abstracts;
using Shared.Exceptions;

namespace AccountService.Application.Commands.CompanyRole
{
    public record GetCompanyRoleByIdCommand(Guid UserId, string CompanyId, string RoleId) : IRequest<UserCompanyRoleDto>;

    public class GetCompanyRoleByIdCommandValidator : AbstractValidator<GetCompanyRoleByIdCommand>;

    public class GetCompanyRoleByIdCommandHandler : IRequestHandler<GetCompanyRoleByIdCommand, UserCompanyRoleDto>
    {
        private readonly IUserCompanyRoleRepository _userCompanyRoleRepository;

        public GetCompanyRoleByIdCommandHandler(IUserCompanyRoleRepository userCompanyRoleRepository)
        {
            _userCompanyRoleRepository = userCompanyRoleRepository;
        }

        //Get only the user company roles created by the user
        public async Task<UserCompanyRoleDto> Handle(GetCompanyRoleByIdCommand request, CancellationToken cancellationToken)
        {
            var query = _userCompanyRoleRepository.GetQueryableIncludingProperties();

            query = query.Where(ucr => ucr.Company.Id == request.CompanyId);
            query = query.Where(ucr => ucr.RoleId == request.RoleId);

            var userCompanyRole = await _userCompanyRoleRepository.GetFilteredUserCompanyRoleAsync(query, cancellationToken)
                                  ?? throw new NotFoundException($"Role with id: {request.RoleId} not found");

            var userCompanyRoleDto = UserCompanyRoleDto.CreateDto(userCompanyRole);

            return userCompanyRoleDto;
        }
    }
}