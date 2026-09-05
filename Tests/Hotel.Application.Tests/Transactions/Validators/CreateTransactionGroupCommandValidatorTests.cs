using FluentValidation.TestHelper;
using Hotel.Application.Transactions.Commands;
using Hotel.Application.Transactions.Validators;
using Hotel.Domain.Transactions.Enums;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Validators;

public class CreateTransactionGroupCommandValidatorTests
{
    private readonly CreateTransactionGroupCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand("ROOM", "Room Charges", TransactionType.Charge);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyCode_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand("", "Room Charges", TransactionType.Charge);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validate_WithCodeExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand(new string('A', 21), "Room Charges", TransactionType.Charge);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand("ROOM", "", TransactionType.Charge);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand("ROOM", new string('A', 101), TransactionType.Charge);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithInvalidType_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand("ROOM", "Room Charges", (TransactionType)999);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Type);
    }
}