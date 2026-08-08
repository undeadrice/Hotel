using Hotel.Domain.Reservations.Entities;

namespace Hotel.Domain.Reservations.Services;

public interface IRoomReservationService
{
    Task<Reservation> CreateReservation(
        Guid creatorId,
        Guid roomId,
        DateTime startDate,
        DateTime endDate,
        IEnumerable<Guid> guestIds,
        CancellationToken cancellationToken = default);
}