using FluentValidation.TestHelper;
using Hotel.Application.NumberCycles.Queries;
using Hotel.Application.NumberCycles.Validators;
using Xunit;

namespace Hotel.Application.Tests.NumberCycles.Validators;

public class GetNumberCycleByIdQueryValidatorTests
{
    private readonly GetNumberCycleByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_WithValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetNumberCycleByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetNumberCycleByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}