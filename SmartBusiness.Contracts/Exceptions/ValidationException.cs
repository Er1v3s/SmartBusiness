using SmartBusiness.Contracts.Errors;

namespace SmartBusiness.Contracts.Exceptions
{
    public class CustomValidationException : Exception
    {
        public List<ValidationError> ValidationErrors { get; set; }

        public CustomValidationException(List<ValidationError> validationErrors)
        {
            ValidationErrors = validationErrors;
        }
    }
}
