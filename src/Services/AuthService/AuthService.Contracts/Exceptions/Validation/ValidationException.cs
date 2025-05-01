namespace AuthService.Contracts.Exceptions.Validation
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
