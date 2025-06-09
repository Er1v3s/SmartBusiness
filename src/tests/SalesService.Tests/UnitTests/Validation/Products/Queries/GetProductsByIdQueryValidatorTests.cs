using FluentValidation.TestHelper;
using SalesService.Application.Queries.Products;

namespace SalesService.Tests.UnitTests.Validation.Products.Queries
{
    public class GetProductsByIdQueryValidatorTests
    {
        private readonly GetProductsByIdQueryValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_ProductId_And_CompanyId_Are_Valid()
        {
            var query = new GetProductsByIdQuery("ValidProductId")
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_ProductId_Is_Null_Or_Empty(string productId)
        {
            var query = new GetProductsByIdQuery(productId)
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.ProductId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_CompanyId_Is_Null_Or_Empty(string companyId)
        {
            var query = new GetProductsByIdQuery("ValidProductId")
            {
                CompanyId = companyId
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }
    }
}