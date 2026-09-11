using FluentAssertions;
using Hotel.Application.Rooming.Queries;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Rooming.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Queries;

public class GetRoomsQueryHandlerTests
{
    private readonly IRoomReadRepository _roomReadRepository;
    private readonly GetRoomsQueryHandler _handler;

    public GetRoomsQueryHandlerTests()
    {
        _roomReadRepository = Substitute.For<IRoomReadRepository>();
        _handler = new GetRoomsQueryHandler(_roomReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnRoomsFromRepository()
    {
        // Arrange
        var query = new GetRoomsQuery();

        var expected = (IReadOnlyCollection<RoomListDto>)
        [
            new RoomListDto(Guid.NewGuid(), "101", "Standard"),
            new RoomListDto(Guid.NewGuid(), "102", "Deluxe"),
        ];

        _roomReadRepository.GetAll(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
        await _roomReadRepository.Received(1).GetAll(Arg.Any<CancellationToken>());
    }
}