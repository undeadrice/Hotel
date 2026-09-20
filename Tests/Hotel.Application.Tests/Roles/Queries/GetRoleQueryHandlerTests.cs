using FluentAssertions;
using Hotel.Application.Roles.Dtos;
using Hotel.Application.Roles.Queries;
using Hotel.Application.Roles.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Roles.Queries;

public class GetRoleQueryHandlerTests
{
    private readonly IRoleService _roleService;
    private readonly GetRoleQueryHandler _handler;

    public GetRoleQueryHandlerTests()
    {
        _roleService = Substitute.For<IRoleService>();
        _handler = new GetRoleQueryHandler(_roleService);
    }

    [Fact]
    public async Task Handle_ShouldCallGetWithRequestId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetRoleQuery(id);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _roleService.Received(1).Get(id);
    }

    [Fact]
    public async Task Handle_ShouldReturnRoleFromService()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetRoleQuery(id);

        var expected = new RoleDto(
            id,
            "Admin",
            new List<string> { "RoleCreate", "RoleEdit" });

        _roleService.Get(id).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
    }
}