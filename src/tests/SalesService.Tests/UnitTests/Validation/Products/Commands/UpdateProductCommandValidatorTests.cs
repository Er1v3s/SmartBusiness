using FluentValidation.TestHelper;
using SalesService.Application.Commands.Products;

namespace SalesService.Tests.UnitTests.Validation.Products.Commands
{
    public class UpdateProductCommandValidatorTests
    {
        private readonly UpdateProductCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_All_Fields_Are_Valid()
        {
            var command = new UpdateProductCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: "Valid Category",
                Price: 99.99m,
                Tax: 10
            )
            {
                ProductId = "ValidProductId",
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
            var command = new UpdateProductCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: "Valid Category",
                Price: 99.99m,
                Tax: 10
            )
            {
                ProductId = productId,
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
            var command = new UpdateProductCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: "Valid Category",
                Price: 99.99m,
                Tax: 10
            )
            {
                ProductId = "ValidProductId",
                CompanyId = companyId
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }

        public static IEnumerable<object[]> InvalidNames => new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { "ab" }, // Too short
            new object[] { new string('a', 101) } // Too long
        };

        [Theory]
        [MemberData(nameof(InvalidNames))]
        public void Should_Fail_When_Name_Is_Invalid(string name)
        {
            var command = new UpdateProductCommand(
                Name: name,
                Description: "Valid Description",
                Category: "Valid Category",
                Price: 99.99m,
                Tax: 10
            )
            {
                ProductId = "ValidProductId",
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        public static IEnumerable<object[]> InvalidDescriptions => new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { "ab" }, // Too short
            new object[] { new string('a', 501) } // Too long
        };

        [Theory]
        [MemberData(nameof(InvalidDescriptions))]
        public void Should_Fail_When_Description_Is_Invalid(string description)
        {
            var command = new UpdateProductCommand(
                Name: "Valid Name",
                Description: description,
                Category: "Valid Category",
                Price: 99.99m,
                Tax: 10
            )
            {
                ProductId = "ValidProductId",
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        public static IEnumerable<object[]> InvalidCategories => new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { "ab" }, // Too short
            new object[] { new string('a', 51) } // Too long
        };

        [Theory]
        [MemberData(nameof(InvalidCategories))]
        public void Should_Fail_When_Category_Is_Invalid(string category)
        {
            var command = new UpdateProductCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: category,
                Price: 99.99m,
                Tax: 10
            )
            {
                ProductId = "ValidProductId",
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }

        [Theory]
        [InlineData(0)] // Too low value
        [InlineData(-1)] // Negative value
        [InlineData(1000000000)] // Too high value
        [InlineData(99.999)] // Too many decimal places
        public void Should_Fail_When_Price_Is_Invalid(decimal price)
        {
            var command = new UpdateProductCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: "Valid Category",
                Price: price,
                Tax: 10
            )
            {
                ProductId = "ValidProductId",
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Theory]
        [InlineData(-1)] // Too low value
        [InlineData(101)] // Too high value
        public void Should_Fail_When_Tax_Is_Invalid(int tax)
        {
            var command = new UpdateProductCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: "Valid Category",
                Price: 99.99m,
                Tax: tax
            )
            {
                ProductId = "ValidProductId",
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Tax);
        }
    }
}