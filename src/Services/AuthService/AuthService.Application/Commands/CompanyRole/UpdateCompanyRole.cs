using MediatR;
using AuthService.Application.Abstracts;
using AuthService.Application.Commands.Abstracts;
using AuthService.Contracts.DTOs;
using AuthService.Contracts.Exceptions.Auth;
using AuthService.Contracts.Exceptions.Users;
using AuthService.Domain.Entities;

namespace AuthService.Application.Commands.CompanyRole
{
    public record UpdateCompanyRoleCommand(Guid UserId, string CompanyId, string Name) : UserCompanyRoleCommand(Name), 
        IRequest<UserCompanyRoleDto>;

    public class UpdateCompanyCommandValidator : UserCompanyRoleCommandValidator<UpdateCompanyRoleCommand>;
    
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyRoleCommand, UserCompanyRoleDto>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserCompanyRoleRepository _userCompanyRoleRepository;

        public UpdateCompanyCommandHandler(
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IUserCompanyRoleRepository userCompanyRoleRepository
            )
        {
            _companyRepository = companyRepository; 
            _userRepository = userRepository;
            _userCompanyRoleRepository = userCompanyRoleRepository;
        }
        
        public async Task<UserCompanyRoleDto> Handle(UpdateCompanyRoleCommand request, CancellationToken cancellationToken)
        {
            var query = _userRepository.GetQueryableIncludingProperties();
            query = query.Where(u => u.Id == request.UserId);

            // Check if the user exists
            var user = await _userRepository.GetFilteredUserAsync(query, cancellationToken)
                       ?? throw new UserNotFoundException("User not found");
            
            // Check if the user is the owner of the company
            // Controller decorator check this, but user might have "Owner" claim of the other company
            var isUserOwnerOfCompany = await _companyRepository.IsUserOwnerOfCompanyAsync(user.Id, request.CompanyId);
            if (!isUserOwnerOfCompany)
                throw new ForbiddenException("User should be owner of the company");
            
            // 

            await _userCompanyRoleRepository.UpdateUserCompanyRoleAsync(ucr);

            var userCompanyRoleDto = new UserCompanyRoleDto
            {
                UserId = ucr.UserId,
                Username = ucr.User.Username
            };
            
            return userCompanyRoleDto;
        }
    }
}