using FluentAssertions;
using Hotel.Application.Rooming.Queries;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Rooming.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Queries;

public class GetRoomTypeByIdQueryHandlerTests
{
    private readonly IRoomTypeReadRepository _roomTypeReadRepository;
    private readonly GetRoomTypeByIdQueryHandler _handler;

    public GetRoomTypeByIdQueryHandlerTests()
    {
        _roomTypeReadRepository = Substitute.For<IRoomTypeReadRepository>();
        _handler = new GetRoomTypeByIdQueryHandler(_roomTypeReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnRoomTypeFromRepository()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var query = new GetRoomTypeByIdQuery(roomTypeId);

        var expected = new RoomTypeDto(roomTypeId, "Standard", "A standard room.");

        _roomTypeReadRepository.GetById(roomTypeId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
        await _roomTypeReadRepository.Received(1).GetById(roomTypeId, Arg.Any<CancellationToken>());
    }
}