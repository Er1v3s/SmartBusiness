using AccountService.Application.Commands.Account;
using FluentValidation.TestHelper;

namespace AccountService.Tests.UnitTests.Validation.Users;

public class UpdateUserCommandValidatorUnitTests
{
    private readonly UpdateUserCommandValidator _validator;
    
    public UpdateUserCommandValidatorUnitTests()
    {
        _validator = new UpdateUserCommandValidator();
    }

    public static IEnumerable<object[]> InvalidUsernames()
    {
        yield return [new string('a', 51)]; // max 50 characters
    }

    [Theory]
    [InlineData("ab")] // min 3 characters
    [MemberData(nameof(InvalidUsernames))]
    public void Validate_ForInvalidUsername_ShouldReturnValidationError(string invalidUsername)
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid() , invalidUsername, "testEmail@gmail.com");

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
        var command = new UpdateUserCommand(Guid.NewGuid(), invalidUsername, "testEmail@gmail.com");

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
        var command = new UpdateUserCommand(Guid.NewGuid(), "test", invalidEmail);

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
        var command = new UpdateUserCommand(Guid.NewGuid(), "test", invalidEmail);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.Email);
    }
}