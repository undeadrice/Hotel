using Hotel.Application.Roles.Commands;
using Hotel.Application.Roles.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Roles.Commands;

public class UpdateRoleCommandHandlerTests
{
    private readonly IRoleService _roleService;
    private readonly UpdateRoleCommandHandler _handler;

    public UpdateRoleCommandHandlerTests()
    {
        _roleService = Substitute.For<IRoleService>();
        _handler = new UpdateRoleCommandHandler(_roleService);
    }

    [Fact]
    public async Task Handle_ShouldCallRoleServiceUpdateWithMappedArguments()
    {
        // Arrange
        var id = Guid.NewGuid();
        const string name = "Admin";
        var permissions = new List<string> { "RoleCreate", "RoleEdit" };

        var command = new UpdateRoleCommand(id, name, permissions);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _roleService.Received(1).Update(id, name, permissions);
    }
}