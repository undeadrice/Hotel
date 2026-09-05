using FluentValidation.TestHelper;
using Hotel.Application.Guests.Queries;
using Hotel.Application.Guests.Validators;
using Xunit;

namespace Hotel.Application.Tests.Guests.Validators;

public class SearchGuestsQueryValidatorTests
{
    private readonly SearchGuestsQueryValidator _validator = new();

    [Fact]
    public void Validate_WithName_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new SearchGuestsQuery("John", null, null, null);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithPhone_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new SearchGuestsQuery(null, "123456789", null, null);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmail_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new SearchGuestsQuery(null, null, "john.doe@example.com", null);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithDocumentNumber_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new SearchGuestsQuery(null, null, null, "ABC123");

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithNoCriteria_ShouldHaveValidationError()
    {
        // Arrange
        var query = new SearchGuestsQuery(null, null, null, null);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }
}