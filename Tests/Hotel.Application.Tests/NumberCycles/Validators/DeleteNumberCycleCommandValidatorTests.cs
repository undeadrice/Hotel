using FluentValidation.TestHelper;
using Hotel.Application.NumberCycles.Commands;
using Hotel.Application.NumberCycles.Validators;
using Xunit;

namespace Hotel.Application.Tests.NumberCycles.Validators;

public class DeleteNumberCycleCommandValidatorTests
{
    private readonly DeleteNumberCycleCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeleteNumberCycleCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteNumberCycleCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}