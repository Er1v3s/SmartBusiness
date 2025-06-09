using FluentValidation.TestHelper;
using SalesService.Application.Commands.Products;

namespace SalesService.Tests.UnitTests.Validation.Products.Commands
{
    public class DeleteProductCommandValidatorTests
    {
        private readonly DeleteProductCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_ProductId_And_CompanyId_Are_Valid()
        {
            var command = new DeleteProductCommand("ValidProductId")
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_ProductId_Is_Null_Or_Empty(string productId)
        {
            var command = new DeleteProductCommand(productId)
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.ProductId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_CompanyId_Is_Null_Or_Empty(string companyId)
        {
            var command = new DeleteProductCommand("ValidProductId")
            {
                CompanyId = companyId
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }
    }
}