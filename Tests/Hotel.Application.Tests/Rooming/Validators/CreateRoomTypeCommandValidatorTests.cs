using FluentValidation.TestHelper;
using Hotel.Application.Rooming.Commands;
using Hotel.Application.Rooming.Validators;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Validators;

public class CreateRoomTypeCommandValidatorTests
{
    private readonly CreateRoomTypeCommandValidator _validator = new();

    private static CreateRoomTypeCommand CreateValidCommand() =>
        new("Standard", null);

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Name = "" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Name = new string('a', 101) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithDescriptionExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Description = new string('a', 501) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }
}