using AccountService.Application.Abstracts;
using AccountService.Contracts.Exceptions.Users;
using AccountService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Abstracts;
using Shared.Contracts;
using Shared.Exceptions;

namespace AccountService.Application.Commands.Companies
{
    public record DeleteCompanyCommand(Guid UserId, string CompanyId, string Password) : IRequest;

    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEventBus _eventBus;

        public DeleteCompanyCommandHandler(ICompanyRepository companyRepository, IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IEventBus eventBus)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _eventBus = eventBus;
        }

        // If the user is not the creator of the company, he should not be able to delete it
        public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(request.CompanyId)
                ?? throw new NotFoundException($"Company with ID {request.CompanyId} not found.");

            var user = await _userRepository.GetUserByIdAsync(request.UserId)
                       ?? throw new UserNotFoundException();

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) !=
                PasswordVerificationResult.Success)
                throw new InvalidPasswordException("Incorrect password");

            await _companyRepository.DeleteCompanyAsync(company);

            await _eventBus.PublishAsync(new CompanyDeletedEvent(request.CompanyId), cancellationToken);
        }
    }
}
