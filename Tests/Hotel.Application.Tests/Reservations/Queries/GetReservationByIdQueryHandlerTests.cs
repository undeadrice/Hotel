using FluentAssertions;
using Hotel.Application.Reservations.Queries;
using Hotel.Application.Reservations.Repositories;
using Hotel.Application.Reservations.TransferObjects;
using Hotel.Domain.Reservations.Enums;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Reservations.Queries;

public class GetReservationByIdQueryHandlerTests
{
    private readonly IReservationReadRepository _reservationReadRepository;
    private readonly GetReservationByIdQueryHandler _handler;

    public GetReservationByIdQueryHandlerTests()
    {
        _reservationReadRepository = Substitute.For<IReservationReadRepository>();
        _handler = new GetReservationByIdQueryHandler(_reservationReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryGetById()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetReservationByIdQuery(id);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _reservationReadRepository.Received(1).GetById(id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnReservationFromRepository()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetReservationByIdQuery(id);

        var expected = new ReservationDto(
            id,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "RES-1",
            new DateOnly(2026, 8, 10),
            new DateOnly(2026, 8, 12),
            null,
            DateTime.UtcNow,
            ReservationStatus.Reserved,
            [Guid.NewGuid()]);

        _reservationReadRepository.GetById(id, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}