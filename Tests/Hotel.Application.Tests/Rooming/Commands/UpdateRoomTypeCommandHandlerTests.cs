using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Commands;

public class UpdateRoomTypeCommandHandlerTests
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly UpdateRoomTypeCommandHandler _handler;

    public UpdateRoomTypeCommandHandlerTests()
    {
        _roomTypeRepository = Substitute.For<IRoomTypeRepository>();
        _handler = new UpdateRoomTypeCommandHandler(_roomTypeRepository);
    }

    [Fact]
    public async Task Handle_ShouldGetUpdateAndPersistRoomType()
    {
        // Arrange
        var roomType = RoomType.Create("Standard", "Old description.");
        var command = new UpdateRoomTypeCommand(roomType.Id, "Deluxe", "New description.");

        _roomTypeRepository.GetById(roomType.Id).Returns(roomType);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        roomType.Name.Should().Be("Deluxe");
        roomType.Description.Should().Be("New description.");

        await _roomTypeRepository.Received(1).GetById(roomType.Id);
    }
}