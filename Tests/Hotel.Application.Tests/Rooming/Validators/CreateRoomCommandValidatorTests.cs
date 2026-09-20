using FluentValidation.TestHelper;
using Hotel.Application.Rooming.Commands;
using Hotel.Application.Rooming.Validators;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Validators;

public class CreateRoomCommandValidatorTests
{
    private readonly CreateRoomCommandValidator _validator = new();

    private static CreateRoomCommand CreateValidCommand() =>
        new("101", Guid.NewGuid());

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
    public void Validate_WithEmptyRoomNumber_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { RoomNumber = "" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomNumber);
    }

    [Fact]
    public void Validate_WithRoomNumberExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { RoomNumber = new string('1', 21) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomNumber);
    }

    [Fact]
    public void Validate_WithEmptyRoomTypeId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { RoomTypeId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomTypeId);
    }
}