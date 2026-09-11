using FluentValidation.TestHelper;
using Hotel.Application.Auth.Commands;
using Hotel.Application.Auth.Validators;
using Xunit;

namespace Hotel.Application.Tests.Auth.Validators;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new LoginCommand("john.doe@example.com", "password");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new LoginCommand("", "password");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new LoginCommand("not-an-email", "password");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithEmptyPassword_ShouldHaveValidationError()
    {
        // Arrange
        var command = new LoginCommand("john.doe@example.com", "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}