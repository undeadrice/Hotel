using FluentValidation.TestHelper;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Application.FiscalAccounting.Validators;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Validators;

public class CheckOutFiscalAccountCommandValidatorTests
{
    private readonly CheckOutFiscalAccountCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CheckOutFiscalAccountCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyAccountId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CheckOutFiscalAccountCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AccountId);
    }
}