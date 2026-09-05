using FluentValidation.TestHelper;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Application.FiscalAccounting.Validators;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Validators;

public class SettleFolioCommandValidatorTests
{
    private readonly SettleFolioCommandValidator _validator = new();

    private static SettleFolioCommand CreateValidCommand() =>
        new(Guid.NewGuid(), Guid.NewGuid());

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
    public void Validate_WithEmptyAccountId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { AccountId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AccountId);
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
}