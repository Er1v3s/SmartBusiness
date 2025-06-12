using FluentValidation.TestHelper;
using WriteService.Application.Commands.Transactions;

namespace WriteService.Tests.UnitTests.Validation.Transaction.Commands
{
    public class CreateTransactionCommandValidatorTests
    {
        private readonly CreateTransactionCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_All_Fields_Are_Valid()
        {
            var command = new CreateTransactionCommand(
                ItemId: "ValidProductId",
                ItemType: "ValidItemType",
                Quantity: 10,
                TotalAmount: 99.99m,
                Tax: 10
            )
            {
                CompanyId = "ValidCompanyId",
                UserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_ProductId_Is_Null_Or_Empty(string productId)
        {
            var command = new CreateTransactionCommand(
                ItemId: productId,
                ItemType: "ValidItemType",
                Quantity: 10,
                TotalAmount: 99.99m,
                Tax: 10
            )
            {
                CompanyId = "ValidCompanyId",
                UserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.ItemId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1000001)]
        public void Should_Fail_When_Quantity_Is_Invalid(int quantity)
        {
            var command = new CreateTransactionCommand(
                ItemId: "ValidProductId",
                ItemType: "ValidItemType",
                Quantity: quantity,
                TotalAmount: 99.99m,
                Tax: 10
            )
            {
                CompanyId = "ValidCompanyId",
                UserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Quantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1000000001)]
        [InlineData(99.999)] // Too many decimal places
        public void Should_Fail_When_TotalAmount_Is_Invalid(decimal totalAmount)
        {
            var command = new CreateTransactionCommand(
                ItemId: "ValidProductId",
                ItemType: "ValidItemType",
                Quantity: 10,
                TotalAmount: totalAmount,
                Tax: 10
            )
            {
                CompanyId = "ValidCompanyId",
                UserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.TotalAmount);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void Should_Fail_When_Tax_Is_Invalid(int tax)
        {
            var command = new CreateTransactionCommand(
                ItemId: "ValidProductId",
                ItemType: "ValidItemType",
                Quantity: 10,
                TotalAmount: 99.99m,
                Tax: tax
            )
            {
                CompanyId = "ValidCompanyId",
                UserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Tax);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_CompanyId_Is_Null_Or_Empty(string companyId)
        {
            var command = new CreateTransactionCommand(
                ItemId: "ValidProductId",
                ItemType: "ValidItemType",
                Quantity: 10,
                TotalAmount: 99.99m,
                Tax: 10
            )
            {
                CompanyId = companyId,
                UserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }

        [Fact]
        public void Should_Fail_When_UserId_Is_Empty()
        {
            var command = new CreateTransactionCommand(
                ItemId: "ValidProductId",
                ItemType: "ValidItemType",
                Quantity: 10,
                TotalAmount: 99.99m,
                Tax: 10
            )
            {
                CompanyId = "ValidCompanyId",
                UserId = Guid.Empty
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }
    }
}
