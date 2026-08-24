using FluentAssertions;
using Hotel.Application.Roles.Dtos;
using Hotel.Application.Roles.Queries;
using Hotel.Application.Roles.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Roles.Queries;

public class GetRoleListQueryHandlerTests
{
    private readonly IRoleService _roleService;
    private readonly GetRoleListQueryHandler _handler;

    public GetRoleListQueryHandlerTests()
    {
        _roleService = Substitute.For<IRoleService>();
        _handler = new GetRoleListQueryHandler(_roleService);
    }

    [Fact]
    public async Task Handle_ShouldCallGetAll()
    {
        // Arrange
        var query = new GetRoleListQuery();

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _roleService.Received(1).GetAll();
    }

    [Fact]
    public async Task Handle_ShouldReturnRolesFromService()
    {
        // Arrange
        var query = new GetRoleListQuery();

        var expected = new List<RoleSimpleDto>
        {
            new(Guid.NewGuid(), "Admin"),
            new(Guid.NewGuid(), "User"),
        };

        _roleService.GetAll().Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}