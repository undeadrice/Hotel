using FluentValidation.TestHelper;
using Hotel.Application.Users.Commands;
using Hotel.Application.Users.Validators;
using Xunit;

namespace Hotel.Application.Tests.Users.Validators;

public class UpdateUserCommandValidatorTests
{
    private readonly UpdateUserCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateUserCommand(
            Guid.Empty,
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_WithEmptyFirstName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "",
            "Doe",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
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
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            new string('A', 101),
            "Doe",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
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
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "John",
            "",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
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
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "John",
            new string('B', 101),
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
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
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            "john.doe@example.com",
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
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "",
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
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "not-an-email",
            new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}