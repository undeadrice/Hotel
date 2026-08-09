using Hotel.Domain.Folios.Entities;
using Hotel.Domain.Folios.Services;
using Hotel.Domain.Reservations.Entities;

namespace Hotel.Domain.Reservations.Services;

public class RoomReservationService(
    IReservationRepository reservationRepository,
    IFolioRepository folioRepository) : IRoomReservationService
{
    public async Task<Reservation> CreateReservation(
        Guid creatorId,
        Guid roomId,
        Guid ratePlanId,
        DateTime startDate,
        DateTime endDate,
        DateTime? arrivalTime,
        IEnumerable<Guid> guestIds,
        CancellationToken cancellationToken = default)
    {
        var reservation = Reservation.Create(creatorId, roomId, ratePlanId, startDate, endDate, arrivalTime, guestIds);

        await reservationRepository.Add(reservation, cancellationToken);

        var folio = Folio.Create(reservation.Id, creatorId);
        await folioRepository.Add(folio, cancellationToken);

        return reservation;
    }
}