using Hotel.Domain.Reservations.Entities;

namespace Hotel.Domain.Reservations.Services;

public interface IRoomReservationService
{
    Task<Reservation> CreateReservation(
        Guid creatorId,
        Guid roomId,
        Guid ratePlanId,
        DateTime startDate,
        DateTime endDate,
        DateTime? arrivalTime,
        IEnumerable<Guid> guestIds,
        CancellationToken cancellationToken = default);
}
