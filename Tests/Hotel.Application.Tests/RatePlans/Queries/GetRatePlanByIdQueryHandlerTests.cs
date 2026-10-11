using FluentAssertions;
using Hotel.Application.RatePlans.Queries;
using Hotel.Application.RatePlans.Repositories;
using Hotel.Application.RatePlans.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.RatePlans.Queries;

public class GetRatePlanByIdQueryHandlerTests
{
    private readonly IRatePlanReadRepository _ratePlanReadRepository;
    private readonly GetRatePlanByIdQueryHandler _handler;

    public GetRatePlanByIdQueryHandlerTests()
    {
        _ratePlanReadRepository = Substitute.For<IRatePlanReadRepository>();
        _handler = new GetRatePlanByIdQueryHandler(_ratePlanReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnRatePlanFromRepository()
    {
        // Arrange
        var ratePlanId = Guid.NewGuid();
        var query = new GetRatePlanByIdQuery(ratePlanId);

        var expected = new RatePlanDto(
            ratePlanId,
            "Peak Season",
            "Room Charge",
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 12, 31),
            [new RatePlanRoomDto("Standard", 100m)]);

        _ratePlanReadRepository.GetById(ratePlanId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
        await _ratePlanReadRepository.Received(1).GetById(ratePlanId, Arg.Any<CancellationToken>());
    }
}