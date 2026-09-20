using Hotel.Application.Rooming.Commands;
using Hotel.Domain.Rooming.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Commands;

public class UpdateRoomCommandHandlerTests
{
    private readonly IRoomUpdateService _roomUpdateService;
    private readonly UpdateRoomCommandHandler _handler;

    public UpdateRoomCommandHandlerTests()
    {
        _roomUpdateService = Substitute.For<IRoomUpdateService>();
        _handler = new UpdateRoomCommandHandler(_roomUpdateService);
    }

    [Fact]
    public async Task Handle_ShouldCallUpdateRoomWithMappedCommand()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var roomTypeId = Guid.NewGuid();
        var command = new UpdateRoomCommand(roomId, "202", roomTypeId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _roomUpdateService.Received(1)
            .UpdateRoom(roomId, "202", roomTypeId, Arg.Any<CancellationToken>());
    }
}