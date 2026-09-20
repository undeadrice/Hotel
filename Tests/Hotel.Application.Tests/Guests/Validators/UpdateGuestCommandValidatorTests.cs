using FluentValidation.TestHelper;
using Hotel.Application.Guests.Commands;
using Hotel.Application.Guests.Validators;
using Xunit;

namespace Hotel.Application.Tests.Guests.Validators;

public class UpdateGuestCommandValidatorTests
{
    private readonly UpdateGuestCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            "123456789",
            "john.doe@example.com",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.Empty,
            "John",
            "Doe",
            "123456789",
            "john.doe@example.com",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_WithEmptyFirstName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "",
            "Doe",
            "123456789",
            "john.doe@example.com",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_WithFirstNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            new string('A', 101),
            "Doe",
            "123456789",
            "john.doe@example.com",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_WithEmptyLastName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "John",
            "",
            "123456789",
            "john.doe@example.com",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validate_WithLastNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "John",
            new string('B', 101),
            "123456789",
            "john.doe@example.com",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validate_WithEmptyPhone_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            "",
            "john.doe@example.com",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Validate_WithPhoneExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            new string('1', 21),
            "john.doe@example.com",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Validate_WithEmptyEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            "123456789",
            "",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            "123456789",
            "not-an-email",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithEmailExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            "123456789",
            new string('A', 201) + "@example.com",
            "ABC123456");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithEmptyDocumentNumber_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            "123456789",
            "john.doe@example.com",
            "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DocumentNumber);
    }

    [Fact]
    public void Validate_WithDocumentNumberExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateGuestCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            "123456789",
            "john.doe@example.com",
            new string('C', 51));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DocumentNumber);
    }
}