using FluentValidation.TestHelper;
using Hotel.Application.Rooming.Queries;
using Hotel.Application.Rooming.Validators;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Validators;

public class GetAvailableRoomsQueryValidatorTests
{
    private readonly GetAvailableRoomsQueryValidator _validator = new();

    private static GetAvailableRoomsQuery CreateValidQuery() =>
        new(new DateOnly(2026, 8, 10), new DateOnly(2026, 8, 12));

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