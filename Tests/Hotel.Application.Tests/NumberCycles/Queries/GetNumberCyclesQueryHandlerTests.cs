using FluentAssertions;
using Hotel.Application.NumberCycles.Queries;
using Hotel.Application.NumberCycles.Repositories;
using Hotel.Application.NumberCycles.TransferObjects;
using Hotel.Domain.NumberCycles.Enums;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.NumberCycles.Queries;

public class GetNumberCyclesQueryHandlerTests
{
    private readonly INumberCycleReadRepository _numberCycleReadRepository;
    private readonly GetNumberCyclesQueryHandler _handler;

    public GetNumberCyclesQueryHandlerTests()
    {
        _numberCycleReadRepository = Substitute.For<INumberCycleReadRepository>();
        _handler = new GetNumberCyclesQueryHandler(_numberCycleReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnNumberCyclesFromRepository()
    {
        // Arrange
        var query = new GetNumberCyclesQuery();

        var expected = (IReadOnlyCollection<NumberCycleDto>)
        [
            new NumberCycleDto(Guid.NewGuid(), NumberCycleTopic.Reservation, "RES", 1, 1, DateTime.UtcNow),
            new NumberCycleDto(Guid.NewGuid(), NumberCycleTopic.FiscalAccount, "FA", 1, 5, DateTime.UtcNow),
        ];

        _numberCycleReadRepository.GetAll(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
        await _numberCycleReadRepository.Received(1).GetAll(Arg.Any<CancellationToken>());
    }
}