using FluentAssertions;
using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Queries;
using Hotel.Application.Users.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Users.Queries;

public class GetUsersQueryHandlerTests
{
    private readonly IUserService _userService;
    private readonly GetUsersQueryHandler _handler;

    public GetUsersQueryHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _handler = new GetUsersQueryHandler(_userService);
    }

    [Fact]
    public async Task Handle_ShouldCallGetAll()
    {
        // Arrange
        var query = new GetUsersQuery();

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _userService.Received(1).GetAll();
    }

    [Fact]
    public async Task Handle_ShouldReturnUsersFromService()
    {
        // Arrange
        var query = new GetUsersQuery();

        var expected = new List<UserContract>
        {
            new(Guid.NewGuid(), "john.doe@example.com", "John", "Doe", new DateOnly(1990, 1, 1)),
            new(Guid.NewGuid(), "jane.doe@example.com", "Jane", "Doe", new DateOnly(1992, 3, 15)),
        };

        _userService.GetAll().Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}