using FluentValidation.TestHelper;
using SalesService.Application.Commands.Services;

namespace SalesService.Tests.UnitTests.Validation.Services.Commands
{
    public class CreateServiceCommandValidatorTests
    {
        private readonly CreateServiceCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_All_Fields_Are_Valid()
        {
            var command = new CreateServiceCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: "Valid Category",
                Price: 99.99m,
                Tax: 10,
                Duration: 120
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_CompanyId_Is_Null_Or_Empty(string companyId)
        {
            var command = new CreateServiceCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: "Valid Category",
                Price: 99.99m,
                Tax: 10,
                Duration: 120
            )
            {
                CompanyId = companyId
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }

        public static IEnumerable<object[]> InvalidNames => new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { "ab" },
            new object[] { new string('a', 101) }
        };

        [Theory]
        [MemberData(nameof(InvalidNames))]
        public void Should_Fail_When_Name_Is_Invalid(string name)
        {
            var command = new CreateServiceCommand(
                Name: name,
                Description: "Valid Description",
                Category: "Valid Category",
                Price: 99.99m,
                Tax: 10,
                Duration: 120
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        public static IEnumerable<object[]> InvalidDescriptions => new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { "ab" },
            new object[] { new string('a', 501) }
        };

        [Theory]
        [MemberData(nameof(InvalidDescriptions))]
        public void Should_Fail_When_Description_Is_Invalid(string description)
        {
            var command = new CreateServiceCommand(
                Name: "Valid Name",
                Description: description,
                Category: "Valid Category",
                Price: 99.99m,
                Tax: 10,
                Duration: 120
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        public static IEnumerable<object[]> InvalidCategories => new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { "ab" },
            new object[] { new string('a', 51) }
        };

        [Theory]
        [MemberData(nameof(InvalidCategories))]
        public void Should_Fail_When_Category_Is_Invalid(string category)
        {
            var command = new CreateServiceCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: category,
                Price: 99.99m,
                Tax: 10,
                Duration: 120
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1000000000)]
        [InlineData(99.999)]
        public void Should_Fail_When_Price_Is_Invalid(decimal price)
        {
            var command = new CreateServiceCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: "Valid Category",
                Price: price,
                Tax: 10,
                Duration: 120
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void Should_Fail_When_Tax_Is_Invalid(int tax)
        {
            var command = new CreateServiceCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: "Valid Category",
                Price: 99.99m,
                Tax: tax,
                Duration: 120
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Tax);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(601)]
        public void Should_Fail_When_Duration_Is_Invalid(int duration)
        {
            var command = new CreateServiceCommand(
                Name: "Valid Name",
                Description: "Valid Description",
                Category: "Valid Category",
                Price: 99.99m,
                Tax: 10,
                Duration: duration
            )
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Duration);
        }
    }
}
