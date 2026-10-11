using FluentValidation.TestHelper;
using Hotel.Application.FiscalAccounting.Queries;
using Hotel.Application.FiscalAccounting.Validators;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Validators;

public class GetFiscalAccountByIdQueryValidatorTests
{
    private readonly GetFiscalAccountByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_WithValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetFiscalAccountByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetFiscalAccountByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}