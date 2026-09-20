using FluentValidation.TestHelper;
using Hotel.Application.Reservations.Commands;
using Hotel.Application.Reservations.Validators;
using Xunit;

namespace Hotel.Application.Tests.Reservations.Validators;

public class CheckInReservationCommandValidatorTests
{
    private readonly CheckInReservationCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CheckInReservationCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyReservationId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CheckInReservationCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ReservationId);
    }
}