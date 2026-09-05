using FluentValidation.TestHelper;
using Hotel.Application.NumberCycles.Commands;
using Hotel.Application.NumberCycles.Validators;
using Hotel.Domain.NumberCycles.Enums;
using Xunit;

namespace Hotel.Application.Tests.NumberCycles.Validators;

public class CreateNumberCycleCommandValidatorTests
{
    private readonly CreateNumberCycleCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateNumberCycleCommand(NumberCycleTopic.Reservation, "RES", 0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithInvalidTopic_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateNumberCycleCommand((NumberCycleTopic)999, "RES", 0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Topic);
    }

    [Fact]
    public void Validate_WithEmptyPrefix_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateNumberCycleCommand(NumberCycleTopic.Reservation, "", 0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Prefix);
    }

    [Fact]
    public void Validate_WithPrefixExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateNumberCycleCommand(
            NumberCycleTopic.Reservation,
            new string('A', 21),
            0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Prefix);
    }

    [Fact]
    public void Validate_WithNegativeStartIndex_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateNumberCycleCommand(NumberCycleTopic.Reservation, "RES", -1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StartIndex);
    }
}