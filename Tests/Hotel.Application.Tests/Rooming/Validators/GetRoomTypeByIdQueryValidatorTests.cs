using FluentValidation.TestHelper;
using Hotel.Application.Rooming.Queries;
using Hotel.Application.Rooming.Validators;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Validators;

public class GetRoomTypeByIdQueryValidatorTests
{
    private readonly GetRoomTypeByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_WithValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetRoomTypeByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetRoomTypeByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}