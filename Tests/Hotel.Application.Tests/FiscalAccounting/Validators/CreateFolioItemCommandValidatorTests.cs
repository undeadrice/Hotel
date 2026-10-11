using FluentValidation.TestHelper;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Application.FiscalAccounting.Validators;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Validators;

public class CreateFolioItemCommandValidatorTests
{
    private readonly CreateFolioItemCommandValidator _validator = new();

    private static CreateFolioItemCommand CreateValidCommand() =>
        new(
            Guid.NewGuid(),
            "Room charge",
            1,
            100m,
            Guid.NewGuid());

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyFolioId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { FolioId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FolioId);
    }

    [Fact]
    public void Validate_WithEmptyDescription_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Description = "" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithDescriptionExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Description = new string('A', 501) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithNonPositiveQuantity_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Quantity = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void Validate_WithNegativeAmount_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Amount = -1m };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Validate_WithEmptyTransactionCodeId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { TransactionCodeId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TransactionCodeId);
    }
}