using FluentValidation.TestHelper;
using Hotel.Application.RatePlans.Queries;
using Hotel.Application.RatePlans.Validators;
using Xunit;

namespace Hotel.Application.Tests.RatePlans.Validators;

public class GetRatePlanByIdQueryValidatorTests
{
    private readonly GetRatePlanByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_WithValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetRatePlanByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetRatePlanByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}