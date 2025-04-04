using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBusiness.Contracts.Errors;
using SmartBusiness.Contracts.Exceptions;

namespace SmartBusiness.Api.Handlers
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = CreateProblemDetails(exception);

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private static ProblemDetails CreateProblemDetails(Exception exception)
        {
            ProblemDetails problemDetails = exception switch
            {
                NotFoundException => CreateProblemDetails(StatusCodes.Status404NotFound, "Not Found", exception.Message),
                DbUpdateException => CreateProblemDetails(StatusCodes.Status409Conflict, "User with this email address already exist", exception.Message),
                CustomValidationException => CreateProblemDetails(StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
                _ => CreateProblemDetails(StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occured")
            };

            if (exception is CustomValidationException customValidationException)
                problemDetails.Extensions["errors"] = customValidationException.ValidationErrors;

            return problemDetails;
        }

        private static ProblemDetails CreateProblemDetails(int status, string title, string detail)
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