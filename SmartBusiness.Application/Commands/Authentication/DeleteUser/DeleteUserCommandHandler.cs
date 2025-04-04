using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartBusiness.Contracts.Errors;
using SmartBusiness.Infrastructure;

namespace SmartBusiness.Application.Commands.Authentication.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly SmartBusinessDbContext _dbContext;
        public DeleteUserCommandHandler(SmartBusinessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if(user == null)
                throw new NotFoundException("User not found");

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
