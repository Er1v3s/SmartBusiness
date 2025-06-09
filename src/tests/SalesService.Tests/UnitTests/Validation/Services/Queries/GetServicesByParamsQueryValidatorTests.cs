using FluentValidation.TestHelper;
using SalesService.Application.Queries.Services;

namespace SalesService.Tests.UnitTests.Validation.Services.Queries
{
    public class GetServicesByParamsQueryValidatorTests
    {
        private readonly GetServicesByParamsQueryValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_All_Fields_Are_Valid()
        {
            var query = new GetServicesByParamsQuery(
                Id: "ValidServiceId",
                Name: "ValidName",
                Category: "ValidCategory",
                MinPrice: 10.00m,
                MaxPrice: 100.00m,
                MinDuration: 30,
                MaxDuration: 120
            )
            {
                CompanyId = "12345678901234567"
            };

            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_CompanyId_Is_Invalid(string companyId)
        {
            var query = new GetServicesByParamsQuery(
                Id: "ValidServiceId",
                Name: "ValidName",
                Category: "ValidCategory",
                MinPrice: 10.00m,
                MaxPrice: 100.00m,
                MinDuration: 30,
                MaxDuration: 120
            )
            {
                CompanyId = companyId
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }

        [Theory]
        [InlineData("Invalid@Name")]
        [InlineData("Name With Spaces")]
        [InlineData("123!@#")]
        public void Should_Fail_When_Name_Is_Invalid(string name)
        {
            var query = new GetServicesByParamsQuery(
                Id: "ValidServiceId",
                Name: name,
                Category: "ValidCategory",
                MinPrice: 10.00m,
                MaxPrice: 100.00m,
                MinDuration: 30,
                MaxDuration: 120
            )
            {
                CompanyId = "12345678901234567"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("Invalid@Category")]
        [InlineData("Category With Spaces")]
        [InlineData("123!@#")]
        public void Should_Fail_When_Category_Is_Invalid(string category)
        {
            var query = new GetServicesByParamsQuery(
                Id: "ValidServiceId",
                Name: "ValidName",
                Category: category,
                MinPrice: 10.00m,
                MaxPrice: 100.00m,
                MinDuration: 30,
                MaxDuration: 120
            )
            {
                CompanyId = "12345678901234567"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Should_Fail_When_MinPrice_Is_Invalid(decimal minPrice)
        {
            var query = new GetServicesByParamsQuery(
                Id: "ValidServiceId",
                Name: "ValidName",
                Category: "ValidCategory",
                MinPrice: minPrice,
                MaxPrice: 100.00m,
                MinDuration: 30,
                MaxDuration: 120
            )
            {
                CompanyId = "12345678901234567"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.MinPrice);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_When_MaxPrice_Is_Invalid(decimal maxPrice)
        {
            var query = new GetServicesByParamsQuery(
                Id: "ValidServiceId",
                Name: "ValidName",
                Category: "ValidCategory",
                MinPrice: 10.00m,
                MaxPrice: maxPrice,
                MinDuration: 30,
                MaxDuration: 120
            )
            {
                CompanyId = "12345678901234567"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.MaxPrice);
        }

        [Fact]
        public void Should_Fail_When_MaxPrice_Is_Less_Than_MinPrice()
        {
            var query = new GetServicesByParamsQuery(
                Id: "ValidServiceId",
                Name: "ValidName",
                Category: "ValidCategory",
                MinPrice: 100.00m,
                MaxPrice: 50.00m,
                MinDuration: 30,
                MaxDuration: 120
            )
            {
                CompanyId = "12345678901234567"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.MaxPrice);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Should_Fail_When_MinDuration_Is_Invalid(int minDuration)
        {
            var query = new GetServicesByParamsQuery(
                Id: "ValidServiceId",
                Name: "ValidName",
                Category: "ValidCategory",
                MinPrice: 10.00m,
                MaxPrice: 100.00m,
                MinDuration: minDuration,
                MaxDuration: 120
            )
            {
                CompanyId = "12345678901234567"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.MinDuration);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Should_Fail_When_MaxDuration_Is_Invalid(int maxDuration)
        {
            var query = new GetServicesByParamsQuery(
                Id: "ValidServiceId",
                Name: "ValidName",
                Category: "ValidCategory",
                MinPrice: 10.00m,
                MaxPrice: 100.00m,
                MinDuration: 30,
                MaxDuration: maxDuration
            )
            {
                CompanyId = "12345678901234567"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.MaxDuration);
        }

        [Fact]
        public void Should_Fail_When_MaxDuration_Is_Less_Than_MinDuration()
        {
            var query = new GetServicesByParamsQuery(
                Id: "ValidServiceId",
                Name: "ValidName",
                Category: "ValidCategory",
                MinPrice: 10.00m,
                MaxPrice: 100.00m,
                MinDuration: 120,
                MaxDuration: 30
            )
            {
                CompanyId = "12345678901234567"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.MaxDuration);
        }
    }
}
