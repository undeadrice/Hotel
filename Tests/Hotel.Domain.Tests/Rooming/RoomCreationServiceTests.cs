using FluentAssertions;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Exceptions;
using Hotel.Domain.Rooming.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Domain.Tests.Rooming;

public class RoomCreationServiceTests
{
    private readonly IRoomRepository _roomRepository;
    private readonly RoomCreationService _service;

    public RoomCreationServiceTests()
    {
        _roomRepository = Substitute.For<IRoomRepository>();
        _service = new RoomCreationService(_roomRepository);
    }

    [Fact]
    public async Task CreateRoom_WhenRoomNumberIsUnique_ShouldAddRoomAndReturnIt()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        _roomRepository.ExistsByRoomNumber("101", Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _service.CreateRoom("101", roomTypeId);

        // Assert
        result.RoomNumber.Should().Be("101");
        result.RoomTypeId.Should().Be(roomTypeId);
        result.IsActive.Should().BeTrue();

        await _roomRepository.Received(1).ExistsByRoomNumber("101", Arg.Any<CancellationToken>());
        await _roomRepository.Received(1).Add(Arg.Any<Room>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateRoom_WhenRoomNumberAlreadyExists_ShouldThrowRoomNumberAlreadyExistsException()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        _roomRepository.ExistsByRoomNumber("101", Arg.Any<CancellationToken>()).Returns(true);

        // Act
        Func<Task> act = () => _service.CreateRoom("101", roomTypeId);

        // Assert
        await act.Should().ThrowAsync<RoomNumberAlreadyExistsException>();
        await _roomRepository.Received(1).ExistsByRoomNumber("101", Arg.Any<CancellationToken>());
        await _roomRepository.DidNotReceive().Add(Arg.Any<Room>(), Arg.Any<CancellationToken>());
    }
}