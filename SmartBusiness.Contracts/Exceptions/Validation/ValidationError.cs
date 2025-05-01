namespace AuthService.Contracts.Exceptions.Validation
{
    public class ValidationError
    {
        public string Property { get; set; }
        public string ErrorMessage { get; set; }
    }
}
