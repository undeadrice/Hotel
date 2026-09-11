using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Commands;

public class CreateRoomTypeCommandHandlerTests
{
    private readonly IRoomTypeCreationService _roomTypeCreationService;
    private readonly CreateRoomTypeCommandHandler _handler;

    public CreateRoomTypeCommandHandlerTests()
    {
        _roomTypeCreationService = Substitute.For<IRoomTypeCreationService>();
        _handler = new CreateRoomTypeCommandHandler(_roomTypeCreationService);
    }

    [Fact]
    public async Task Handle_ShouldCallCreateRoomTypeReturnRoomTypeId()
    {
        // Arrange
        var command = new CreateRoomTypeCommand("Deluxe", "A deluxe room.");
        var roomType = RoomType.Create("Deluxe", "A deluxe room.");

        _roomTypeCreationService
            .CreateRoomType("Deluxe", "A deluxe room.", Arg.Any<CancellationToken>())
            .Returns(roomType);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(roomType.Id);
        await _roomTypeCreationService.Received(1)
            .CreateRoomType("Deluxe", "A deluxe room.", Arg.Any<CancellationToken>());
    }
}