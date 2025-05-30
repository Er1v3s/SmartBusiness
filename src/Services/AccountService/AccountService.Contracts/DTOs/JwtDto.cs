namespace AccountService.Contracts.DTOs
{
    public class JwtDto
    {
        public required string JwtToken { get; set; }
        public required DateTime ExpirationDateInUtc { get; set; }
    }
}
