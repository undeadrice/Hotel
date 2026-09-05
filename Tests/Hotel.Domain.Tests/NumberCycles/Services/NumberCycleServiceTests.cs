using FluentAssertions;
using Hotel.Domain.NumberCycles.Entities;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.Domain.NumberCycles.Exceptions;
using Hotel.Domain.NumberCycles.Repositories;
using Hotel.Domain.NumberCycles.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Domain.Tests.NumberCycles.Services;

public class NumberCycleServiceTests
{
    private readonly INumberCycleRepository _numberCycleRepository;
    private readonly NumberCycleService _service;

    public NumberCycleServiceTests()
    {
        _numberCycleRepository = Substitute.For<INumberCycleRepository>();
        _service = new NumberCycleService(_numberCycleRepository);
    }

    [Fact]
    public async Task Create_WhenTopicDoesNotExist_ShouldAddAndReturnCycle()
    {
        // Arrange
        _numberCycleRepository
            .ExistsByTopic(NumberCycleTopic.Reservation, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _service.Create(NumberCycleTopic.Reservation, " res ", 10);

        // Assert
        result.Topic.Should().Be(NumberCycleTopic.Reservation);
        result.Prefix.Should().Be("RES");
        result.StartIndex.Should().Be(10);
        result.CurrentIndex.Should().Be(10);

        await _numberCycleRepository.Received(1).ExistsByTopic(NumberCycleTopic.Reservation, Arg.Any<CancellationToken>());
        await _numberCycleRepository.Received(1).Add(result, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_WhenTopicAlreadyExists_ShouldThrowNumberCycleAlreadyExistsException()
    {
        // Arrange
        _numberCycleRepository
            .ExistsByTopic(NumberCycleTopic.Reservation, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        Func<Task> act = () => _service.Create(NumberCycleTopic.Reservation, "RES", 10);

        // Assert
        await act.Should().ThrowAsync<NumberCycleAlreadyExistsException>();

        await _numberCycleRepository.Received(1).ExistsByTopic(NumberCycleTopic.Reservation, Arg.Any<CancellationToken>());
        await _numberCycleRepository.DidNotReceive().Add(Arg.Any<NumberCycle>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Delete_WhenNoChildren_ShouldDeleteCycle()
    {
        // Arrange
        var cycleId = Guid.NewGuid();
        var cycle = NumberCycle.Create(NumberCycleTopic.FiscalAccount, "FA", 1);

        _numberCycleRepository.GetById(cycleId, Arg.Any<CancellationToken>()).Returns(cycle);
        _numberCycleRepository.CountChildren(NumberCycleTopic.FiscalAccount, Arg.Any<CancellationToken>()).Returns(0);

        // Act
        await _service.Delete(cycleId);

        // Assert
        await _numberCycleRepository.Received(1).GetById(cycleId, Arg.Any<CancellationToken>());
        await _numberCycleRepository.Received(1).CountChildren(NumberCycleTopic.FiscalAccount, Arg.Any<CancellationToken>());
        await _numberCycleRepository.Received(1).Delete(cycle, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Delete_WhenHasChildren_ShouldThrowNumberCycleHasChildrenException()
    {
        // Arrange
        var cycleId = Guid.NewGuid();
        var cycle = NumberCycle.Create(NumberCycleTopic.FiscalAccount, "FA", 1);

        _numberCycleRepository.GetById(cycleId, Arg.Any<CancellationToken>()).Returns(cycle);
        _numberCycleRepository.CountChildren(NumberCycleTopic.FiscalAccount, Arg.Any<CancellationToken>()).Returns(3);

        // Act
        Func<Task> act = () => _service.Delete(cycleId);

        // Assert
        await act.Should().ThrowAsync<NumberCycleHasChildrenException>();

        await _numberCycleRepository.Received(1).CountChildren(NumberCycleTopic.FiscalAccount, Arg.Any<CancellationToken>());
        await _numberCycleRepository.DidNotReceive().Delete(Arg.Any<NumberCycle>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task NextIdentifier_ShouldReturnCycleIdentifier()
    {
        // Arrange
        var cycle = NumberCycle.Create(NumberCycleTopic.Reservation, "RES", 5);

        _numberCycleRepository.GetByTopic(NumberCycleTopic.Reservation, Arg.Any<CancellationToken>()).Returns(cycle);

        // Act
        var identifier = await _service.NextIdentifier(NumberCycleTopic.Reservation);

        // Assert
        identifier.Should().Be("RES-5");

        await _numberCycleRepository.Received(1).GetByTopic(NumberCycleTopic.Reservation, Arg.Any<CancellationToken>());
    }
}