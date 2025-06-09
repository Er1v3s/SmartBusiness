using FluentValidation.TestHelper;
using SalesService.Application.Queries.Services;

namespace SalesService.Tests.UnitTests.Validation.Services.Queries
{
    public class GetServicesByIdQueryValidatorTests
    {
        private readonly GetServicesByIdQueryValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_ServiceId_And_CompanyId_Are_Valid()
        {
            var query = new GetServicesByIdQuery("ValidServiceId")
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_ServiceId_Is_Null_Or_Empty(string serviceId)
        {
            var query = new GetServicesByIdQuery(serviceId)
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.ServiceId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_CompanyId_Is_Null_Or_Empty(string companyId)
        {
            var query = new GetServicesByIdQuery("ValidServiceId")
            {
                CompanyId = companyId
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }
    }
}