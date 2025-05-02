using MediatR;
using FluentValidation;
using AuthService.Contracts.Exceptions.Validation;

namespace AuthService.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResult = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResult
                .SelectMany(v => v.Errors)
                .Select(x => new ValidationError
                {
                    Property = x.PropertyName,
                    ErrorMessage = x.ErrorMessage
                }).ToList();

            if(failures.Any())
                throw new CustomValidationException(failures);

            var response = await next(cancellationToken);

            return response;
        }
    }
}
