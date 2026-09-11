using FluentAssertions;
using Hotel.Application.Roles.Commands;
using Hotel.Application.Roles.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Roles.Commands;

public class CreateRoleCommandHandlerTests
{
    private readonly IRoleService _roleService;
    private readonly CreateRoleCommandHandler _handler;

    public CreateRoleCommandHandlerTests()
    {
        _roleService = Substitute.For<IRoleService>();
        _handler = new CreateRoleCommandHandler(_roleService);
    }

    [Fact]
    public async Task Handle_ShouldCallRoleServiceCreateWithMappedArguments()
    {
        // Arrange
        const string name = "Admin";
        var permissions = new List<string> { "RoleCreate", "RoleEdit" };

        var command = new CreateRoleCommand(name, permissions);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _roleService.Received(1).Create(name, permissions);
    }

    [Fact]
    public async Task Handle_ShouldReturnGuidFromRoleService()
    {
        // Arrange
        const string name = "Admin";
        var permissions = new List<string> { "RoleCreate", "RoleEdit" };
        var expected = Guid.NewGuid();

        var command = new CreateRoleCommand(name, permissions);

        _roleService.Create(name, permissions).Returns(expected);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
    }
}