using FluentAssertions;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Exceptions;
using Hotel.Domain.Rooming.Repositories;
using Hotel.Domain.Rooming.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Domain.Tests.Rooming.Services;

public class RoomTypeCreationServiceTests
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly RoomTypeCreationService _service;

    public RoomTypeCreationServiceTests()
    {
        _roomTypeRepository = Substitute.For<IRoomTypeRepository>();
        _service = new RoomTypeCreationService(_roomTypeRepository);
    }

    [Fact]
    public async Task CreateRoomType_WhenNameIsUnique_ShouldAddRoomTypeAndReturnIt()
    {
        // Arrange
        _roomTypeRepository.ExistsByName("Deluxe", Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _service.CreateRoomType("Deluxe", "A deluxe room");

        // Assert
        result.Name.Should().Be("Deluxe");
        result.Description.Should().Be("A deluxe room");
        result.IsActive.Should().BeTrue();

        await _roomTypeRepository.Received(1).ExistsByName("Deluxe", Arg.Any<CancellationToken>());
        await _roomTypeRepository.Received(1).Add(Arg.Any<RoomType>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateRoomType_WhenNameAlreadyExists_ShouldThrowRoomTypeNameAlreadyExistsException()
    {
        // Arrange
        _roomTypeRepository.ExistsByName("Deluxe", Arg.Any<CancellationToken>()).Returns(true);

        // Act
        Func<Task> act = () => _service.CreateRoomType("Deluxe", "A deluxe room");

        // Assert
        await act.Should().ThrowAsync<RoomTypeNameAlreadyExistsException>();
        await _roomTypeRepository.Received(1).ExistsByName("Deluxe", Arg.Any<CancellationToken>());
        await _roomTypeRepository.DidNotReceive().Add(Arg.Any<RoomType>(), Arg.Any<CancellationToken>());
    }
}