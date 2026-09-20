using FluentAssertions;
using Hotel.Application.RatePlans.Queries;
using Hotel.Application.RatePlans.Repositories;
using Hotel.Application.RatePlans.TransferObjects;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Rooming.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.RatePlans.Queries;

public class GetRatePlansByRoomTypeQueryHandlerTests
{
    private readonly IRoomReadRepository _roomReadRepository;
    private readonly IRatePlanReadRepository _ratePlanReadRepository;
    private readonly GetRatePlansByRoomTypeQueryHandler _handler;

    public GetRatePlansByRoomTypeQueryHandlerTests()
    {
        _roomReadRepository = Substitute.For<IRoomReadRepository>();
        _ratePlanReadRepository = Substitute.For<IRatePlanReadRepository>();
        _handler = new GetRatePlansByRoomTypeQueryHandler(_roomReadRepository, _ratePlanReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnRatePlansUsingRoomsRoomTypeIdAndDateRange()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var roomTypeId = Guid.NewGuid();
        var startDate = new DateOnly(2026, 9, 1);
        var endDate = new DateOnly(2026, 9, 5);
        var query = new GetRatePlansByRoomTypeQuery(roomId, startDate, endDate);

        var room = new RoomDto(roomId, "101", roomTypeId, "Standard", true);

        var expected = (IReadOnlyCollection<RatePlanListSimpleDto>)
        [
            new RatePlanListSimpleDto(Guid.NewGuid(), "Peak Season"),
            new RatePlanListSimpleDto(Guid.NewGuid(), "Off Season"),
        ];

        _roomReadRepository.GetById(roomId, Arg.Any<CancellationToken>()).Returns(room);
        _ratePlanReadRepository
            .GetByRoomTypeId(roomTypeId, startDate, endDate, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
        await _roomReadRepository.Received(1).GetById(roomId, Arg.Any<CancellationToken>());
        await _ratePlanReadRepository.Received(1)
            .GetByRoomTypeId(roomTypeId, startDate, endDate, Arg.Any<CancellationToken>());
    }
}