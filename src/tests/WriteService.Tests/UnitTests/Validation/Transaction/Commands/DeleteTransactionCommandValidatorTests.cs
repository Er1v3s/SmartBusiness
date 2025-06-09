using FluentValidation.TestHelper;
using WriteService.Application.Commands.Transactions;

namespace WriteService.Tests.UnitTests.Validation.Transaction.Commands
{
    public class DeleteTransactionCommandValidatorTests
    {
        private readonly DeleteTransactionCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_TransactionId_And_CompanyId_Are_Valid()
        {
            var command = new DeleteTransactionCommand("ValidTransactionId")
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_TransactionId_Is_Null_Or_Empty(string transactionId)
        {
            var command = new DeleteTransactionCommand(transactionId)
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.TransactionId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_CompanyId_Is_Null_Or_Empty(string companyId)
        {
            var command = new DeleteTransactionCommand("ValidTransactionId")
            {
                CompanyId = companyId
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }
    }
}