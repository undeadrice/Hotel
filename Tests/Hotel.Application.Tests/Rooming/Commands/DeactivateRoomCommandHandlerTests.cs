using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Commands;

public class DeactivateRoomCommandHandlerTests
{
    private readonly IRoomRepository _roomRepository;
    private readonly DeactivateRoomCommandHandler _handler;

    public DeactivateRoomCommandHandlerTests()
    {
        _roomRepository = Substitute.For<IRoomRepository>();
        _handler = new DeactivateRoomCommandHandler(_roomRepository);
    }

    [Fact]
    public async Task Handle_ShouldGetRoomAndDeactivateIt()
    {
        // Arrange
        var room = Room.Create("101", Guid.NewGuid());

        _roomRepository.GetById(room.Id, Arg.Any<CancellationToken>()).Returns(room);

        var command = new DeactivateRoomCommand(room.Id);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        room.IsActive.Should().BeFalse();
        await _roomRepository.Received(1).GetById(room.Id, Arg.Any<CancellationToken>());
    }
}
