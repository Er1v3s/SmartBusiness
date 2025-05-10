namespace Shared.Exceptions
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
