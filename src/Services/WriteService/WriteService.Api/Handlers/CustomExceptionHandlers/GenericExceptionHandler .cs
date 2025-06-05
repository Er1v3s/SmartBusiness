using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;

namespace WriteService.Api.Handlers.CustomExceptionHandlers
{
    public class GenericExceptionHandler : ICustomExceptionHandler
    {
        private static readonly HashSet<Type> HandledExceptions = new()
        {
            typeof(NotFoundException),
        };

        public virtual bool CanHandle(Exception ex) => HandledExceptions.Contains(ex.GetType());


        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = CreateProblemDetails(exception);

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        protected virtual ProblemDetails CreateProblemDetails(Exception exception)
        {
            ProblemDetails problemDetails = exception switch
            {
                NotFoundException => CreateProblemDetails(StatusCodes.Status404NotFound, "Not Found", exception.Message),
                _ => CreateProblemDetails(StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occured")
            };

            if (exception is CustomValidationException customValidationException)
                problemDetails.Extensions["errors"] = customValidationException.ValidationErrors;

            return problemDetails;
        }

        protected virtual ProblemDetails CreateProblemDetails(int status, string title, string detail)
        {
            return new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail
            };
        }
    }
}