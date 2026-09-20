using FluentAssertions;
using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Queries;
using Hotel.Application.Users.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Users.Queries;

public class GetUserQueryHandlerTests
{
    private readonly IUserService _userService;
    private readonly GetUserQueryHandler _handler;

    public GetUserQueryHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _handler = new GetUserQueryHandler(_userService);
    }

    [Fact]
    public async Task Handle_ShouldCallGetByIdWithRequestId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetUserQuery(id);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _userService.Received(1).GetById(id);
    }

    [Fact]
    public async Task Handle_ShouldReturnUserFromService()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetUserQuery(id);

        var expected = new UserWithRolesContract(
            id,
            "john.doe@example.com",
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            new List<Guid> { Guid.NewGuid(), Guid.NewGuid() });

        _userService.GetById(id).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
    }
}