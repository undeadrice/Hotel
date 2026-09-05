using FluentAssertions;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.Rooming.Entities;

public class RoomTests
{
    private readonly Guid _roomTypeId = Guid.NewGuid();

    [Fact]
    public void Create_WithValidArguments_ShouldCreateActiveRoom()
    {
        // Act
        var room = Room.Create("101", _roomTypeId);

        // Assert
        room.Id.Should().NotBe(Guid.Empty);
        room.RoomNumber.Should().Be("101");
        room.RoomTypeId.Should().Be(_roomTypeId);
        room.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidRoomNumber_ShouldThrowRoomNumberRequiredException(string? roomNumber)
    {
        // Act
        Action act = () => Room.Create(roomNumber!, _roomTypeId);

        // Assert
        act.Should().Throw<RoomNumberRequiredException>();
    }

    [Fact]
    public void UpdateRoomNumber_WithValidNumber_ShouldChangeRoomNumber()
    {
        // Arrange
        var room = Room.Create("101", _roomTypeId);

        // Act
        room.UpdateRoomNumber("202");

        // Assert
        room.RoomNumber.Should().Be("202");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateRoomNumber_WithInvalidNumber_ShouldThrowRoomNumberRequiredException(string? roomNumber)
    {
        // Arrange
        var room = Room.Create("101", _roomTypeId);

        // Act
        Action act = () => room.UpdateRoomNumber(roomNumber!);

        // Assert
        act.Should().Throw<RoomNumberRequiredException>();
    }

    [Fact]
    public void ChangeRoomType_ShouldUpdateRoomTypeId()
    {
        // Arrange
        var room = Room.Create("101", _roomTypeId);
        var newRoomTypeId = Guid.NewGuid();

        // Act
        room.ChangeRoomType(newRoomTypeId);

        // Assert
        room.RoomTypeId.Should().Be(newRoomTypeId);
    }

    [Fact]
    public void Deactivate_WhenActive_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var room = Room.Create("101", _roomTypeId);

        // Act
        room.Deactivate();

        // Assert
        room.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Deactivate_WhenAlreadyInactive_ShouldThrowRoomStatusChangeInvalidException()
    {
        // Arrange
        var room = Room.Create("101", _roomTypeId);
        room.Deactivate();

        // Act
        Action act = () => room.Deactivate();

        // Assert
        act.Should().Throw<RoomStatusChangeInvalidException>();
    }
}