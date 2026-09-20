using FluentAssertions;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Exceptions;
using Hotel.Domain.Rooming.Repositories;
using Hotel.Domain.Rooming.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Domain.Tests.Rooming.Services;

public class RoomTypeUpdateServiceTests
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly RoomTypeUpdateService _service;

    public RoomTypeUpdateServiceTests()
    {
        _roomTypeRepository = Substitute.For<IRoomTypeRepository>();
        _service = new RoomTypeUpdateService(_roomTypeRepository);
    }

    [Fact]
    public async Task UpdateRoomType_WhenNameIsUnique_ShouldUpdateNameAndDescription()
    {
        // Arrange
        var roomType = RoomType.Create("Standard", "Old description");
        _roomTypeRepository.GetById(roomType.Id, Arg.Any<CancellationToken>()).Returns(roomType);
        _roomTypeRepository.ExistsByNameExcluding(roomType.Id, "Deluxe", Arg.Any<CancellationToken>()).Returns(false);

        // Act
        await _service.UpdateRoomType(roomType.Id, "Deluxe", "New description");

        // Assert
        roomType.Name.Should().Be("Deluxe");
        roomType.Description.Should().Be("New description");

        await _roomTypeRepository.Received(1).GetById(roomType.Id, Arg.Any<CancellationToken>());
        await _roomTypeRepository.Received(1).ExistsByNameExcluding(roomType.Id, "Deluxe", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateRoomType_WhenNameAlreadyExists_ShouldThrowAndNotModifyRoomType()
    {
        // Arrange
        var roomType = RoomType.Create("Standard", "Old description");
        _roomTypeRepository.GetById(roomType.Id, Arg.Any<CancellationToken>()).Returns(roomType);
        _roomTypeRepository.ExistsByNameExcluding(roomType.Id, "Deluxe", Arg.Any<CancellationToken>()).Returns(true);

        // Act
        Func<Task> act = () => _service.UpdateRoomType(roomType.Id, "Deluxe", "New description");

        // Assert
        await act.Should().ThrowAsync<RoomTypeNameAlreadyExistsException>();
        roomType.Name.Should().Be("Standard");
        roomType.Description.Should().Be("Old description");

        await _roomTypeRepository.Received(1).ExistsByNameExcluding(roomType.Id, "Deluxe", Arg.Any<CancellationToken>());
    }
}