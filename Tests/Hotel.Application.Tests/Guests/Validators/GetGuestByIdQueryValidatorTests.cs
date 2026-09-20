using FluentValidation.TestHelper;
using Hotel.Application.Guests.Queries;
using Hotel.Application.Guests.Validators;
using Xunit;

namespace Hotel.Application.Tests.Guests.Validators;

public class GetGuestByIdQueryValidatorTests
{
    private readonly GetGuestByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_WithValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetGuestByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetGuestByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}