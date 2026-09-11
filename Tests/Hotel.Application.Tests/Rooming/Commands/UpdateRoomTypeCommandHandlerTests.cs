using Hotel.Application.Rooming.Commands;
using Hotel.Domain.Rooming.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Commands;

public class UpdateRoomTypeCommandHandlerTests
{
    private readonly IRoomTypeUpdateService _roomTypeUpdateService;
    private readonly UpdateRoomTypeCommandHandler _handler;

    public UpdateRoomTypeCommandHandlerTests()
    {
        _roomTypeUpdateService = Substitute.For<IRoomTypeUpdateService>();
        _handler = new UpdateRoomTypeCommandHandler(_roomTypeUpdateService);
    }

    [Fact]
    public async Task Handle_ShouldCallUpdateRoomType()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var command = new UpdateRoomTypeCommand(roomTypeId, "Deluxe", "New description.");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _roomTypeUpdateService.Received(1)
            .UpdateRoomType(roomTypeId, "Deluxe", "New description.", Arg.Any<CancellationToken>());
    }
}