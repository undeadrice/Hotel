using FluentValidation.TestHelper;
using Hotel.Application.Rooming.Commands;
using Hotel.Application.Rooming.Validators;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Validators;

public class DeactivateRoomCommandValidatorTests
{
    private readonly DeactivateRoomCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeactivateRoomCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyRoomId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeactivateRoomCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomId);
    }
}