using FluentAssertions;
using Hotel.Application.RatePlans.Queries;
using Hotel.Application.RatePlans.Repositories;
using Hotel.Application.RatePlans.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.RatePlans.Queries;

public class GetRatePlansQueryHandlerTests
{
    private readonly IRatePlanReadRepository _ratePlanReadRepository;
    private readonly GetRatePlansQueryHandler _handler;

    public GetRatePlansQueryHandlerTests()
    {
        _ratePlanReadRepository = Substitute.For<IRatePlanReadRepository>();
        _handler = new GetRatePlansQueryHandler(_ratePlanReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnRatePlansFromRepository()
    {
        // Arrange
        var query = new GetRatePlansQuery();

        var expected = (IReadOnlyCollection<RatePlanListDto>)
        [
            new RatePlanListDto(Guid.NewGuid(), "Peak Season", Guid.NewGuid(), new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31), true),
            new RatePlanListDto(Guid.NewGuid(), "Off Season", Guid.NewGuid(), new DateOnly(2026, 11, 1), new DateOnly(2027, 2, 28), false),
        ];

        _ratePlanReadRepository.GetAll(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
        await _ratePlanReadRepository.Received(1).GetAll(Arg.Any<CancellationToken>());
    }
}