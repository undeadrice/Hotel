using FluentAssertions;
using Hotel.Application.Rooming.Queries;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Rooming.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Queries;

public class GetAvailableRoomsQueryHandlerTests
{
    private readonly IRoomReadRepository _roomReadRepository;
    private readonly GetAvailableRoomsQueryHandler _handler;

    public GetAvailableRoomsQueryHandlerTests()
    {
        _roomReadRepository = Substitute.For<IRoomReadRepository>();
        _handler = new GetAvailableRoomsQueryHandler(_roomReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnAvailableRoomsForDateRange()
    {
        // Arrange
        var startDate = new DateOnly(2026, 9, 1);
        var endDate = new DateOnly(2026, 9, 5);
        var query = new GetAvailableRoomsQuery(startDate, endDate);

        var expected = (IReadOnlyCollection<RoomListDto>)
        [
            new RoomListDto(Guid.NewGuid(), "101", "Standard"),
        ];

        _roomReadRepository
            .GetAvailable(startDate, endDate, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
        await _roomReadRepository.Received(1)
            .GetAvailable(startDate, endDate, Arg.Any<CancellationToken>());
    }
}