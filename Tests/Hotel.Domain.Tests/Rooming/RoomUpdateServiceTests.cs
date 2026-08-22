using FluentAssertions;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Exceptions;
using Hotel.Domain.Rooming.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Domain.Tests.Rooming;

public class RoomUpdateServiceTests
{
    private readonly IRoomRepository _roomRepository;
    private readonly RoomUpdateService _service;

    public RoomUpdateServiceTests()
    {
        _roomRepository = Substitute.For<IRoomRepository>();
        _service = new RoomUpdateService(_roomRepository);
    }

    [Fact]
    public async Task UpdateRoom_WhenRoomNumberAndRoomTypeUnchanged_ShouldNotModifyRoomOrCheckExistence()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var room = Room.Create("101", roomTypeId);

        _roomRepository.GetById(room.Id, Arg.Any<CancellationToken>()).Returns(room);

        // Act
        await _service.UpdateRoom(room.Id, "101", roomTypeId);

        // Assert
        room.RoomNumber.Should().Be("101");
        room.RoomTypeId.Should().Be(roomTypeId);

        await _roomRepository.Received(1).GetById(room.Id, Arg.Any<CancellationToken>());
        await _roomRepository.DidNotReceive().ExistsByRoomNumber(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateRoom_WhenOnlyRoomNumberChangedAndUnique_ShouldUpdateRoomNumberOnly()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var room = Room.Create("101", roomTypeId);

        _roomRepository.GetById(room.Id, Arg.Any<CancellationToken>()).Returns(room);
        _roomRepository.ExistsByRoomNumber("202", Arg.Any<CancellationToken>()).Returns(false);

        // Act
        await _service.UpdateRoom(room.Id, "202", roomTypeId);

        // Assert
        room.RoomNumber.Should().Be("202");
        room.RoomTypeId.Should().Be(roomTypeId);

        await _roomRepository.Received(1).ExistsByRoomNumber("202", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateRoom_WhenOnlyRoomTypeChanged_ShouldUpdateRoomTypeWithoutCheckingExistence()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var newRoomTypeId = Guid.NewGuid();
        var room = Room.Create("101", roomTypeId);

        _roomRepository.GetById(room.Id, Arg.Any<CancellationToken>()).Returns(room);

        // Act
        await _service.UpdateRoom(room.Id, "101", newRoomTypeId);

        // Assert
        room.RoomNumber.Should().Be("101");
        room.RoomTypeId.Should().Be(newRoomTypeId);

        await _roomRepository.Received(1).GetById(room.Id, Arg.Any<CancellationToken>());
        await _roomRepository.DidNotReceive().ExistsByRoomNumber(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateRoom_WhenRoomNumberChangedAndAlreadyExists_ShouldThrowAndNotModifyRoom()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var room = Room.Create("101", roomTypeId);

        _roomRepository.GetById(room.Id, Arg.Any<CancellationToken>()).Returns(room);
        _roomRepository.ExistsByRoomNumber("202", Arg.Any<CancellationToken>()).Returns(true);

        // Act
        Func<Task> act = () => _service.UpdateRoom(room.Id, "202", roomTypeId);

        // Assert
        await act.Should().ThrowAsync<RoomNumberAlreadyExistsException>();
        room.RoomNumber.Should().Be("101");
        room.RoomTypeId.Should().Be(roomTypeId);

        await _roomRepository.Received(1).ExistsByRoomNumber("202", Arg.Any<CancellationToken>());
    }
}