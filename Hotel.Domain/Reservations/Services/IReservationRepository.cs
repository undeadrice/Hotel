using Hotel.Domain.Reservations.Entities;

namespace Hotel.Domain.Reservations.Services;

public interface IReservationRepository
{
    Task Add(Reservation reservation, CancellationToken token = default);

    Task Update(Reservation reservation, CancellationToken token = default);

    Task<Reservation?> FindById(Guid id, CancellationToken token = default);

    Task<Reservation> GetById(Guid id, CancellationToken token = default);

    Task<bool> HasOverlappingReservation(Guid roomId, DateOnly startDate, DateOnly endDate, CancellationToken token = default);

    Task<IReadOnlyCollection<Reservation>> GetForEndOfDay(DateOnly businessDate, CancellationToken token = default);
}
