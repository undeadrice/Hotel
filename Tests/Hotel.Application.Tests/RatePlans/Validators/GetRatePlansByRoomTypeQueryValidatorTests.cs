using FluentValidation.TestHelper;
using Hotel.Application.RatePlans.Queries;
using Hotel.Application.RatePlans.Validators;
using Xunit;

namespace Hotel.Application.Tests.RatePlans.Validators;

public class GetRatePlansByRoomTypeQueryValidatorTests
{
    private readonly GetRatePlansByRoomTypeQueryValidator _validator = new();

    private static GetRatePlansByRoomTypeQuery CreateValidQuery() =>
        new(Guid.NewGuid(), new DateOnly(2026, 8, 10), new DateOnly(2026, 8, 12));

    [Fact]
    public void Validate_WithValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = CreateValidQuery();

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyRoomId_ShouldHaveValidationError()
    {
        // Arrange
        var query = CreateValidQuery() with { RoomId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomId);
    }

    [Fact]
    public void Validate_WithDefaultStartDate_ShouldHaveValidationError()
    {
        // Arrange
        var query = CreateValidQuery() with { StartDate = default };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void Validate_WithDefaultEndDate_ShouldHaveValidationError()
    {
        // Arrange
        var query = CreateValidQuery() with { EndDate = default };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void Validate_WhenStartDateIsNotBeforeEndDate_ShouldHaveValidationError()
    {
        // Arrange
        var query = CreateValidQuery() with
        {
            StartDate = new DateOnly(2026, 8, 12),
            EndDate = new DateOnly(2026, 8, 10)
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }
}