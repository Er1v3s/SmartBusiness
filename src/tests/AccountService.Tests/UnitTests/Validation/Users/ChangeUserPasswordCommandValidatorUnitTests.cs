using AccountService.Application.Commands.Account;
using FluentValidation.TestHelper;

namespace AccountService.Tests.UnitTests.Validation.Users;

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