using FluentValidation.TestHelper;
using Hotel.Application.RatePlans.Commands;
using Hotel.Application.RatePlans.Validators;
using Xunit;

namespace Hotel.Application.Tests.RatePlans.Validators;

public class CreateRatePlanCommandValidatorTests
{
    private readonly CreateRatePlanCommandValidator _validator = new();

    private static CreateRatePlanCommand CreateValidCommand() =>
        new(
            "Peak Season",
            Guid.NewGuid(),
            new DateOnly(2026, 8, 10),
            new DateOnly(2026, 8, 12),
            new List<CreateRatePlanRoomCommand> { new(Guid.NewGuid(), 100m) });

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
    public void Validate_WithEmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Name = "" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
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

    [Fact]
    public void Validate_WithDefaultStartDate_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { StartDate = default };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void Validate_WithDefaultEndDate_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { EndDate = default };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void Validate_WhenStartDateIsNotBeforeEndDate_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with
        {
            StartDate = new DateOnly(2026, 8, 12),
            EndDate = new DateOnly(2026, 8, 10)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void Validate_WithEmptyRooms_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Rooms = new List<CreateRatePlanRoomCommand>() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Rooms);
    }

    [Fact]
    public void Validate_WithRoomWithEmptyRoomTypeId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with
        {
            Rooms = new List<CreateRatePlanRoomCommand> { new(Guid.Empty, 100m) }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Rooms[0].RoomTypeId");
    }

    [Fact]
    public void Validate_WithRoomWithNegativePrice_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with
        {
            Rooms = new List<CreateRatePlanRoomCommand> { new(Guid.NewGuid(), -1m) }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Rooms[0].Price");
    }
}