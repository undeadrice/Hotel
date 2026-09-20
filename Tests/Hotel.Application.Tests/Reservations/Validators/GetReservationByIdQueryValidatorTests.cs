using FluentValidation.TestHelper;
using Hotel.Application.Reservations.Queries;
using Hotel.Application.Reservations.Validators;
using Xunit;

namespace Hotel.Application.Tests.Reservations.Validators;

public class GetReservationByIdQueryValidatorTests
{
    private readonly GetReservationByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_WithValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetReservationByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetReservationByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}