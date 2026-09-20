using FluentValidation.TestHelper;
using Hotel.Application.Users.Commands;
using Hotel.Application.Users.Validators;
using Xunit;

namespace Hotel.Application.Tests.Users.Validators;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
            "secret1",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyFirstName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "",
            "Doe",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
            "secret1",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_WithFirstNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateUserCommand(
            new string('A', 101),
            "Doe",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
            "secret1",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_WithEmptyLastName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John",
            "",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
            "secret1",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validate_WithLastNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John",
            new string('B', 101),
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
            "secret1",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validate_WithFutureDateOfBirth_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John",
            "Doe",
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            "john.doe@example.com",
            "secret1",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Validate_WithEmptyEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "",
            "secret1",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "not-an-email",
            "secret1",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithEmptyPassword_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
            "",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_WithShortPassword_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
            "12345",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}