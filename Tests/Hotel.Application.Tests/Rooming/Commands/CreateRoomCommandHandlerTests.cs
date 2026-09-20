using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Commands;

public class CreateRoomCommandHandlerTests
{
    private readonly IRoomCreationService _roomCreationService;
    private readonly CreateRoomCommandHandler _handler;

    public CreateRoomCommandHandlerTests()
    {
        _roomCreationService = Substitute.For<IRoomCreationService>();
        _handler = new CreateRoomCommandHandler(_roomCreationService);
    }

    [Fact]
    public async Task Handle_ShouldCallCreateRoomWithMappedCommandAndReturnRoomId()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var command = new CreateRoomCommand("101", roomTypeId);
        var room = Room.Create("101", roomTypeId);

        _roomCreationService
            .CreateRoom("101", roomTypeId, Arg.Any<CancellationToken>())
            .Returns(room);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(room.Id);
        await _roomCreationService.Received(1)
            .CreateRoom("101", roomTypeId, Arg.Any<CancellationToken>());
    }
}
