using FluentValidation.TestHelper;
using Hotel.Application.Transactions.Commands;
using Hotel.Application.Transactions.Validators;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Validators;

public class UpdateTransactionCodeCommandValidatorTests
{
    private readonly UpdateTransactionCodeCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.NewGuid(), "ROOM", "Room Charge");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.Empty, Guid.NewGuid(), "ROOM", "Room Charge");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_WithEmptyTransactionGroupId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.Empty, "ROOM", "Room Charge");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TransactionGroupId);
    }

    [Fact]
    public void Validate_WithEmptyCode_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.NewGuid(), "", "Room Charge");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validate_WithCodeExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.NewGuid(), new string('A', 21), "Room Charge");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.NewGuid(), "ROOM", "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.NewGuid(), "ROOM", new string('A', 101));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}