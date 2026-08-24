using FluentAssertions;
using Hotel.Application.Rooming.Queries;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Rooming.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Queries;

public class GetRoomByIdQueryHandlerTests
{
    private readonly IRoomReadRepository _roomReadRepository;
    private readonly GetRoomByIdQueryHandler _handler;

    public GetRoomByIdQueryHandlerTests()
    {
        _roomReadRepository = Substitute.For<IRoomReadRepository>();
        _handler = new GetRoomByIdQueryHandler(_roomReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnRoomFromRepository()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var roomTypeId = Guid.NewGuid();
        var query = new GetRoomByIdQuery(roomId);

        var expected = new RoomDto(roomId, "101", roomTypeId, "Standard", true);

        _roomReadRepository.GetById(roomId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
        await _roomReadRepository.Received(1).GetById(roomId, Arg.Any<CancellationToken>());
    }
}