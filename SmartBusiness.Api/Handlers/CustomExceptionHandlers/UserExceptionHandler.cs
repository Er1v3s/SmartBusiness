using Microsoft.AspNetCore.Mvc;
using SmartBusiness.Contracts.Exceptions.Users;

namespace SmartBusiness.Api.Handlers.CustomExceptionHandlers
{
    public class UserExceptionHandler : GenericExceptionHandler
    {
        private static readonly HashSet<Type> HandledExceptions = new()
        {
            typeof(UserNotFoundException),
            typeof(UserAlreadyExistsException),
            typeof(InvalidPasswordException)
        };

        public override bool CanHandle(Exception ex) => HandledExceptions.Contains(ex.GetType());

        protected override ProblemDetails CreateProblemDetails(Exception exception)
        {
            return exception switch
            {
                UserNotFoundException => CreateProblemDetails(StatusCodes.Status404NotFound, "Not Found", exception.Message),
                UserAlreadyExistsException => CreateProblemDetails(StatusCodes.Status409Conflict, "Conflict", exception.Message),
                InvalidPasswordException => CreateProblemDetails(StatusCodes.Status400BadRequest, "Bad request", exception.Message),
                _ => base.CreateProblemDetails(exception)
            };
        }
    }

}
