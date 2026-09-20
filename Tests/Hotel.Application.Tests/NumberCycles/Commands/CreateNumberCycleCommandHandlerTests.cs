using FluentAssertions;
using Hotel.Application.NumberCycles.Commands;
using Hotel.Domain.NumberCycles.Entities;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.Domain.NumberCycles.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.NumberCycles.Commands;

public class CreateNumberCycleCommandHandlerTests
{
    private readonly INumberCycleService _numberCycleService;
    private readonly CreateNumberCycleCommandHandler _handler;

    public CreateNumberCycleCommandHandlerTests()
    {
        _numberCycleService = Substitute.For<INumberCycleService>();
        _handler = new CreateNumberCycleCommandHandler(_numberCycleService);
    }

    [Fact]
    public async Task Handle_ShouldCallNumberCycleServiceCreateWithMappedArguments()
    {
        // Arrange
        var command = new CreateNumberCycleCommand(NumberCycleTopic.Reservation, "RES", 10);
        var cycle = NumberCycle.Create(NumberCycleTopic.Reservation, "RES", 10);

        _numberCycleService
            .Create(NumberCycleTopic.Reservation, "RES", 10, Arg.Any<CancellationToken>())
            .Returns(cycle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _numberCycleService.Received(1).Create(
            NumberCycleTopic.Reservation,
            "RES",
            10,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnIdFromCreatedCycle()
    {
        // Arrange
        var command = new CreateNumberCycleCommand(NumberCycleTopic.Reservation, "RES", 10);
        var cycle = NumberCycle.Create(NumberCycleTopic.Reservation, "RES", 10);

        _numberCycleService
            .Create(NumberCycleTopic.Reservation, "RES", 10, Arg.Any<CancellationToken>())
            .Returns(cycle);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(cycle.Id);
    }
}