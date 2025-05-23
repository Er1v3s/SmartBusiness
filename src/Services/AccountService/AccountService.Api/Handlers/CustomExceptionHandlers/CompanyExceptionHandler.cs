using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Handlers.CustomExceptionHandlers
{
    public class CompanyExceptionHandler : GenericExceptionHandler
    {
        private static readonly HashSet<Type> HandledExceptions = new()
        {
            // 
        };

        public override bool CanHandle(Exception ex) => HandledExceptions.Contains(ex.GetType());

        protected override ProblemDetails CreateProblemDetails(Exception exception)
        {
            return exception switch
            {
                _ => base.CreateProblemDetails(exception)
            };
        }
    }
}
