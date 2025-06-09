using FluentValidation.TestHelper;
using ReadService.Application.Queries;

namespace ReadService.Tests.UnitTests.Validation.Queries
{
    public class GetTransactionsRequestValidatorTests
    {
        private readonly GetTransactionsRequestValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_All_Fields_Are_Valid()
        {
            var query = new GetTransactionsByParamsQuery(
                UserId: Guid.NewGuid().ToString(),
                ProductId: "ValidNanoId",
                Quantity: 10,
                MinTotalAmount: 10.00m,
                MaxTotalAmount: 100.00m,
                MinTax: 5,
                MaxTax: 20,
                MinTotalAmountMinusTax: 8.00m,
                MaxTotalAmountMinusTax: 90.00m,
                StartDateTime: DateTime.UtcNow.AddDays(-1),
                EndDateTime: DateTime.UtcNow
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("InvalidGuid")]
        [InlineData("12345")]
        public void Should_Fail_When_UserId_Is_Invalid(string userId)
        {
            var query = new GetTransactionsByParamsQuery(
                UserId: userId,
                ProductId: "ValidNanoId",
                Quantity: 10,
                MinTotalAmount: 10.00m,
                MaxTotalAmount: 100.00m,
                MinTax: 5,
                MaxTax: 20,
                MinTotalAmountMinusTax: 8.00m,
                MaxTotalAmountMinusTax: 90.00m,
                StartDateTime: DateTime.UtcNow.AddDays(-1),
                EndDateTime: DateTime.UtcNow
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Theory]
        [InlineData("Invalid@NanoId")]
        [InlineData("Nano Id")]
        [InlineData("123!@#")]
        public void Should_Fail_When_ProductId_Is_Invalid(string productId)
        {
            var query = new GetTransactionsByParamsQuery(
                UserId: Guid.NewGuid().ToString(),
                ProductId: productId,
                Quantity: 10,
                MinTotalAmount: 10.00m,
                MaxTotalAmount: 100.00m,
                MinTax: 5,
                MaxTax: 20,
                MinTotalAmountMinusTax: 8.00m,
                MaxTotalAmountMinusTax: 90.00m,
                StartDateTime: DateTime.UtcNow.AddDays(-1),
                EndDateTime: DateTime.UtcNow
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.ProductId);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1000001)]
        public void Should_Fail_When_Quantity_Is_Invalid(int quantity)
        {
            var query = new GetTransactionsByParamsQuery(
                UserId: Guid.NewGuid().ToString(),
                ProductId: "ValidNanoId",
                Quantity: quantity,
                MinTotalAmount: 10.00m,
                MaxTotalAmount: 100.00m,
                MinTax: 5,
                MaxTax: 20,
                MinTotalAmountMinusTax: 8.00m,
                MaxTotalAmountMinusTax: 90.00m,
                StartDateTime: DateTime.UtcNow.AddDays(-1),
                EndDateTime: DateTime.UtcNow
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Quantity);
        }

        [Fact]
        public void Should_Fail_When_MaxTotalAmount_Is_Less_Than_MinTotalAmount()
        {
            var query = new GetTransactionsByParamsQuery(
                UserId: Guid.NewGuid().ToString(),
                ProductId: "ValidNanoId",
                Quantity: 10,
                MinTotalAmount: 100.00m,
                MaxTotalAmount: 50.00m,
                MinTax: 5,
                MaxTax: 20,
                MinTotalAmountMinusTax: 8.00m,
                MaxTotalAmountMinusTax: 90.00m,
                StartDateTime: DateTime.UtcNow.AddDays(-1),
                EndDateTime: DateTime.UtcNow
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.MaxTotalAmount);
        }

        [Fact]
        public void Should_Fail_When_MaxTax_Is_Less_Than_MinTax()
        {
            var query = new GetTransactionsByParamsQuery(
                UserId: Guid.NewGuid().ToString(),
                ProductId: "ValidNanoId",
                Quantity: 10,
                MinTotalAmount: 10.00m,
                MaxTotalAmount: 100.00m,
                MinTax: 20,
                MaxTax: 5,
                MinTotalAmountMinusTax: 8.00m,
                MaxTotalAmountMinusTax: 90.00m,
                StartDateTime: DateTime.UtcNow.AddDays(-1),
                EndDateTime: DateTime.UtcNow
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.MaxTax);
        }

        [Fact]
        public void Should_Fail_When_MaxTotalAmountMinusTax_Is_Less_Than_MinTotalAmountMinusTax()
        {
            var query = new GetTransactionsByParamsQuery(
                UserId: Guid.NewGuid().ToString(),
                ProductId: "ValidNanoId",
                Quantity: 10,
                MinTotalAmount: 10.00m,
                MaxTotalAmount: 100.00m,
                MinTax: 5,
                MaxTax: 20,
                MinTotalAmountMinusTax: 90.00m,
                MaxTotalAmountMinusTax: 8.00m,
                StartDateTime: DateTime.UtcNow.AddDays(-1),
                EndDateTime: DateTime.UtcNow
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.MaxTotalAmountMinusTax);
        }

        [Fact]
        public void Should_Fail_When_EndDateTime_Is_Before_StartDateTime()
        {
            var query = new GetTransactionsByParamsQuery(
                UserId: Guid.NewGuid().ToString(),
                ProductId: "ValidNanoId",
                Quantity: 10,
                MinTotalAmount: 10.00m,
                MaxTotalAmount: 100.00m,
                MinTax: 5,
                MaxTax: 20,
                MinTotalAmountMinusTax: 8.00m,
                MaxTotalAmountMinusTax: 90.00m,
                StartDateTime: DateTime.UtcNow,
                EndDateTime: DateTime.UtcNow.AddDays(-1)
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.EndDateTime);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_CompanyId_Is_Null_Or_Empty(string companyId)
        {
            var query = new GetTransactionsByParamsQuery(
                UserId: Guid.NewGuid().ToString(),
                ProductId: "ValidNanoId",
                Quantity: 10,
                MinTotalAmount: 10.00m,
                MaxTotalAmount: 100.00m,
                MinTax: 5,
                MaxTax: 20,
                MinTotalAmountMinusTax: 8.00m,
                MaxTotalAmountMinusTax: 90.00m,
                StartDateTime: DateTime.UtcNow.AddDays(-1),
                EndDateTime: DateTime.UtcNow
            )
            {
                CompanyId = companyId
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }
    }
}
