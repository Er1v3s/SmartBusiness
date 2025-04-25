using FluentValidation.TestHelper;
using SmartBusiness.Application.Commands.Users.Create;

namespace SmartBusiness.Tests.UnitTests.Validations.Users;

public class CreateUserCommandValidatorUnitTests
{
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorUnitTests()
    {
        _validator = new CreateUserCommandValidator();
    }

    public static IEnumerable<object[]> InvalidUsernames()
    {
        yield return [new string('a', 51)]; // max 50 characters
    }

    [Theory]
    [InlineData(null)] // not null 
    [InlineData("")] // not empty
    [InlineData("ab")] // min 3 characters
    [MemberData(nameof(InvalidUsernames))]
    public void Validate_ForInvalidUsername_ShouldReturnValidationError(string invalidUsername)
    {
        // Arrange
        var command = new CreateUserCommand(invalidUsername, "testEmail@gmail.com", "testPasswordHash");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Username);
    }

    [Theory]
    [InlineData("TestUsername")] // correct username
    public void Validate_ForValidUsername_ShouldNotReturnValidationError(string invalidUsername)
    {
        // Arrange
        var command = new CreateUserCommand(invalidUsername, "testEmail@gmail.com", "testPasswordHash");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.Username);
    }

    public static IEnumerable<object[]> InvalidEmails()
    {
        yield return [$"{new string('t', 48)}@{new string('b',48)}.com"]; // invalid email format (too long)
    }

    [Theory]
    [InlineData(null)] // not null 
    [InlineData("")] // not empty
    [InlineData("test.example.com")] // invalid email format (missing @)
    [InlineData("test@example")] // invalid email format (missing domain)
    [InlineData("test@example,com")] // invalid email format (incorrect separator)
    [InlineData("test@example.x")] // invalid email format (domain too short)
    [InlineData("test@example .com")] // invalid email format (whitespace before domain)
    [InlineData("test@example!.com")] // invalid email format (special character in domain)
    [MemberData(nameof(InvalidEmails))]
    public void Validate_ForInvalidEmails_ShouldReturnValidationError(string invalidEmail)
    {
        // Arrange
        var command = new CreateUserCommand("test", invalidEmail, "testPasswordHash");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Email);
    }

    [Theory]
    [InlineData("test@email.com")] // correct email
    public void Validate_ForValidEmails_ShouldNotReturnValidationError(string invalidEmail)
    {
        // Arrange
        var command = new CreateUserCommand("test", invalidEmail, "testPasswordHash");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.Email);
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
        var command = new CreateUserCommand("test", "test@email.com", invalidPassword);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Password);
    }

    [Theory]
    [InlineData("!@TestPassword123")] // correct password
    public void Validate_ForValidPassword_ShouldNotReturnValidationError(string invalidPassword)
    {
        // Arrange
        var command = new CreateUserCommand("test", "test@email.com", invalidPassword);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.Password);
    }
}