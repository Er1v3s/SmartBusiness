using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartBusiness.Contracts.Errors;
using SmartBusiness.Domain.Entities;
using SmartBusiness.Infrastructure;

namespace SmartBusiness.Application.Commands.UserCommands.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
    {
        private readonly SmartBusinessDbContext _dbContext;

        public UpdateUserCommandHandler(SmartBusinessDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
        }

        public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if(existingUser == null)
                throw new NotFoundException("User not found");

            if(request.Email != existingUser.Email)
                existingUser.Email = request.Email;

            if (request.Username != existingUser.Username)
                existingUser.Username = request.Username;


            _dbContext.Users.Update(existingUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return existingUser.Username;
        }
    }
}
