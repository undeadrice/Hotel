using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.Domain.Rooming.Repositories;
using NSubstitute;
using Xunit;
using Hotel.Domain.Rooming.Entities;

namespace Hotel.Application.Tests.Rooming.Commands;

public class CreateRoomTypeCommandHandlerTests
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly CreateRoomTypeCommandHandler _handler;

    public CreateRoomTypeCommandHandlerTests()
    {
        _roomTypeRepository = Substitute.For<IRoomTypeRepository>();
        _handler = new CreateRoomTypeCommandHandler(_roomTypeRepository);
    }

    [Fact]
    public async Task Handle_ShouldAddRoomTypeAndReturnItsId()
    {
        // Arrange
        var command = new CreateRoomTypeCommand("Deluxe", "A deluxe room.");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();

        await _roomTypeRepository.Received(1).Add(Arg.Is<RoomType>(roomType =>
            roomType != null &&
            roomType.Name == "Deluxe" &&
            roomType.Description == "A deluxe room." &&
            roomType.Id == result));
    }
}