using FluentValidation.TestHelper;
using SmartBusiness.Application.Commands.Users.ChangePassword;

namespace SmartBusiness.Tests.UnitTests.Validations.Users;

public class ChangeUserPasswordCommandValidatorUnitTests
{
    private readonly ChangeUserPasswordCommandValidator _validator;

    public ChangeUserPasswordCommandValidatorUnitTests()
    {
        _validator = new ChangeUserPasswordCommandValidator();
    }

    public static IEnumerable<object[]> InvalidPassword()
    {
        yield return [$"{new string('t', 48)}!1X"]; // too long
    }

    [Theory]
    [InlineData(null)] // not null 
    [InlineData("")] // not empty
    [InlineData("NoSpecialChar123")] // no special character
    [InlineData("nouppercase123!")] // no uppercase letter
    [InlineData("NOLOWERCASE123!")] // no lowercase letter
    [InlineData("NoNumber!")] // no number
    [InlineData("!1Ab")] // too short
    [MemberData(nameof(InvalidPassword))]
    public void Validate_ForInvalidPassword_ShouldReturnValidationError(string invalidPassword)
    {
        // Arrange
        var command = new ChangeUserPasswordCommand(Guid.NewGuid(), "test@email.com", invalidPassword);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.NewPassword);
    }

    [Theory]
    [InlineData("!@TestPassword123")] // correct password
    public void Validate_ForValidPassword_ShouldNotReturnValidationError(string invalidPassword)
    {
        // Arrange
        var command = new ChangeUserPasswordCommand(Guid.NewGuid(), "test@email.com", invalidPassword);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.NewPassword);
    }
}