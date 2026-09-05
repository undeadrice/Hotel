using FluentValidation.TestHelper;
using Hotel.Application.Roles.Commands;
using Hotel.Application.Roles.Validators;
using Xunit;

namespace Hotel.Application.Tests.Roles.Validators;

public class CreateRoleCommandValidatorTests
{
    private readonly CreateRoleCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateRoleCommand(
            "Admin",
            new List<string> { "RoleCreate", "RoleEdit" });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateRoleCommand(
            "",
            new List<string> { "RoleCreate" });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateRoleCommand(
            new string('a', 257),
            new List<string> { "RoleCreate" });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNullPermissions_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateRoleCommand("Admin", null!);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Permissions);
    }

    [Fact]
    public void Validate_WithInvalidPermission_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateRoleCommand(
            "Admin",
            new List<string> { "NotARealPermission" });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Permissions);
    }
}