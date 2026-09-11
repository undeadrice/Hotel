using FluentAssertions;
using Hotel.Application.NumberCycles.Queries;
using Hotel.Application.NumberCycles.Repositories;
using Hotel.Application.NumberCycles.TransferObjects;
using Hotel.Domain.NumberCycles.Enums;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.NumberCycles.Queries;

public class GetNumberCycleByIdQueryHandlerTests
{
    private readonly INumberCycleReadRepository _numberCycleReadRepository;
    private readonly GetNumberCycleByIdQueryHandler _handler;

    public GetNumberCycleByIdQueryHandlerTests()
    {
        _numberCycleReadRepository = Substitute.For<INumberCycleReadRepository>();
        _handler = new GetNumberCycleByIdQueryHandler(_numberCycleReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryGetById()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetNumberCycleByIdQuery(id);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _numberCycleReadRepository.Received(1).GetById(id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNumberCycleFromRepository()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetNumberCycleByIdQuery(id);

        var expected = new NumberCycleDto(id, NumberCycleTopic.Reservation, "RES", 1, 5, DateTime.UtcNow);

        _numberCycleReadRepository.GetById(id, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}