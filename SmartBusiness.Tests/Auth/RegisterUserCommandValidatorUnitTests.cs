using FluentValidation.TestHelper;
using SmartBusiness.Application.Commands.Auth.RegisterUser;

namespace SmartBusiness.Tests.Auth;

public class RegisterUserCommandValidatorUnitTests
{
    private readonly RegisterUserCommandValidator _validator;

    public RegisterUserCommandValidatorUnitTests()
    {
        _validator = new RegisterUserCommandValidator();
    }

    public static IEnumerable<object[]> InvalidUsernames()
    {
        yield return [null]; // not null 
        yield return [""]; // not empty
        yield return ["ab"]; // min 3 characters
        yield return [new string('a', 51)]; // max 50 characters
    }

    [Theory]
    [MemberData(nameof(InvalidUsernames))]
    public void Validate_ForInvalidUsername_ShouldReturnValidationError(string invalidUsername)
    {
        // Arrange
        var command = new RegisterUserCommand(invalidUsername, "testEmail@gmail.com", "testPasswordHash");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Username);
    }

    public static IEnumerable<object[]> InvalidEmails()
    {
        yield return [null]; // not null 
        yield return [""]; // not empty
        yield return ["test.example.com"]; // invalid email format (missing @)
        yield return ["test@example"]; // invalid email format (missing domain)
        yield return ["test@example,com"]; // invalid email format (incorrect separator)
        yield return ["test@example.x"]; // invalid email format (domain too short)
        yield return ["test@example .com"]; // invalid email format (whitespace before domain)
        yield return ["test@example!.com"]; // invalid email format (special character in domain)
        yield return [$"{new string('t', 48)}@{new string('b',48)}.com"]; // invalid email format (too long)
    }

    [Theory]
    [MemberData(nameof(InvalidEmails))]
    public void Validate_ForInvalidEmails_ShouldReturnValidationError(string invalidEmail)
    {
        // Arrange
        var command = new RegisterUserCommand("test", invalidEmail, "testPasswordHash");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Email);
    }

    public static IEnumerable<object[]> InvalidPassword()
    {
        yield return [null]; // not null 
        yield return [""]; // not empty
        yield return ["NoSpecialChar123"]; // no special character
        yield return ["nouppercase123!"]; // no uppercase letter
        yield return ["NOLOWERCASE123!"]; // no lowercase letter
        yield return ["NoNumber!"]; // no number
        yield return ["!1Ab"]; // too short
        yield return [$"{new string('t', 48)}!1X"]; // too long
    }

    [Theory]
    [MemberData(nameof(InvalidPassword))]
    public void Validate_ForInvalidPassword_ShouldReturnValidationError(string invalidPassword)
    {
        // Arrange
        var command = new RegisterUserCommand("test", "test@email.com", invalidPassword);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Password);
    }
}
