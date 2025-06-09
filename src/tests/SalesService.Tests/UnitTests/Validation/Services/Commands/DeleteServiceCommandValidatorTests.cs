using FluentValidation.TestHelper;
using SalesService.Application.Commands.Services;

namespace SalesService.Tests.UnitTests.Validation.Services.Commands
{
    public class DeleteServiceCommandValidatorTests
    {
        private readonly DeleteServiceCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_ServiceId_And_CompanyId_Are_Valid()
        {
            var command = new DeleteServiceCommand("ValidServiceId")
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_ServiceId_Is_Null_Or_Empty(string serviceId)
        {
            var command = new DeleteServiceCommand(serviceId)
            {
                CompanyId = "ValidCompanyId"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.ServiceId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_CompanyId_Is_Null_Or_Empty(string companyId)
        {
            var command = new DeleteServiceCommand("ValidServiceId")
            {
                CompanyId = companyId
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }
    }
}