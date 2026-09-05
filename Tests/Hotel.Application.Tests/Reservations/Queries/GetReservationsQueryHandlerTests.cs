using FluentAssertions;
using Hotel.Application.Reservations.Queries;
using Hotel.Application.Reservations.Repositories;
using Hotel.Application.Reservations.TransferObjects;
using Hotel.Domain.Reservations.Enums;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Reservations.Queries;

public class GetReservationsQueryHandlerTests
{
    private readonly IReservationReadRepository _reservationReadRepository;
    private readonly GetReservationsQueryHandler _handler;

    public GetReservationsQueryHandlerTests()
    {
        _reservationReadRepository = Substitute.For<IReservationReadRepository>();
        _handler = new GetReservationsQueryHandler(_reservationReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryGetAll()
    {
        // Arrange
        var query = new GetReservationsQuery();

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _reservationReadRepository.Received(1).GetAll(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnReservationsFromRepository()
    {
        // Arrange
        var query = new GetReservationsQuery();

        var expected = new List<ReservationListDto>
        {
            new(
                Guid.NewGuid(),
                "RES-1",
                "101",
                "Standard",
                "John Doe",
                new DateOnly(2026, 8, 10),
                new DateOnly(2026, 8, 12),
                null,
                DateTime.UtcNow,
                ReservationStatus.Reserved,
                2)
        };

        _reservationReadRepository.GetAll(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}