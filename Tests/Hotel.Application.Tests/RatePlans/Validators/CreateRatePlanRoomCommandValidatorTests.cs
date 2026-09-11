using FluentValidation.TestHelper;
using Hotel.Application.RatePlans.Commands;
using Hotel.Application.RatePlans.Validators;
using Xunit;

namespace Hotel.Application.Tests.RatePlans.Validators;

public class CreateRatePlanRoomCommandValidatorTests
{
    private readonly CreateRatePlanRoomCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateRatePlanRoomCommand(Guid.NewGuid(), 100m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyRoomTypeId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateRatePlanRoomCommand(Guid.Empty, 100m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomTypeId);
    }

    [Fact]
    public void Validate_WithNegativePrice_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateRatePlanRoomCommand(Guid.NewGuid(), -1m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }
}