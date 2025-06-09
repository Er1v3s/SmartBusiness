using FluentValidation.TestHelper;
using ReadService.Application.Queries;

namespace ReadService.Tests.UnitTests.Validation.Queries
{
    public class GetTransactionByIdQueryValidatorTests
    {
        private readonly GetTransactionByIdQueryValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_TransactionId_And_CompanyId_Are_Valid()
        {
            var query = new GetTransactionByIdQuery("ValidTransactionId")
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_TransactionId_Is_Null_Or_Empty(string transactionId)
        {
            var query = new GetTransactionByIdQuery(transactionId)
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.TransactionId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_CompanyId_Is_Null_Or_Empty(string companyId)
        {
            var query = new GetTransactionByIdQuery("ValidTransactionId")
            {
                CompanyId = companyId
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }
    }
}