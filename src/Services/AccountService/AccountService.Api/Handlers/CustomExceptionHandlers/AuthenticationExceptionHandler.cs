using Microsoft.AspNetCore.Mvc;
using AccountService.Contracts.Exceptions.Auth;

namespace AccountService.Api.Handlers.CustomExceptionHandlers
{
    public class AuthenticationExceptionHandler : GenericExceptionHandler
    {
        private static readonly HashSet<Type> HandledExceptions = new()
        {
            typeof(RefreshTokenException),
            typeof(AuthenticationException),
            typeof(ForbiddenException),
        };

        public override bool CanHandle(Exception ex) => HandledExceptions.Contains(ex.GetType());

        protected override ProblemDetails CreateProblemDetails(Exception exception)
        {
            return exception switch
            {
                RefreshTokenException => CreateProblemDetails(StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message),
                AuthenticationException => CreateProblemDetails(StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message),
                ForbiddenException => CreateProblemDetails(StatusCodes.Status403Forbidden, "Forbidden", exception.Message),
                _ => base.CreateProblemDetails(exception)
            };
        }
    }
}