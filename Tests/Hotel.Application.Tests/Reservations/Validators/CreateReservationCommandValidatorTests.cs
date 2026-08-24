using FluentValidation.TestHelper;
using Hotel.Application.Reservations.Commands;
using Hotel.Application.Reservations.Validators;
using Xunit;

namespace Hotel.Application.Tests.Reservations.Validators;

public class CreateReservationCommandValidatorTests
{
    private readonly CreateReservationCommandValidator _validator = new();

    private static CreateReservationCommand CreateValidCommand() =>
        new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateOnly(2026, 8, 10),
            new DateOnly(2026, 8, 12),
            null,
            new List<Guid> { Guid.NewGuid() });

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
    public void Validate_WithEmptyCreatorId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { CreatorId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CreatorId);
    }

    [Fact]
    public void Validate_WithEmptyRoomId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { RoomId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomId);
    }

    [Fact]
    public void Validate_WithEmptyRatePlanId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { RatePlanId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RatePlanId);
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
    public void Validate_WithEmptyGuestIds_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { GuestIds = new List<Guid>() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GuestIds);
    }
}