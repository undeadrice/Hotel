using Hotel.Domain.Reservations.Enums;

namespace Hotel.Domain.Reservations.Services;

public class ReservationCheckOutService(IReservationRepository reservationRepository) : IReservationCheckOutService
{
    public async Task CheckOut(Guid reservationId, CancellationToken cancellationToken = default)
    {
        var reservation = await reservationRepository.GetById(reservationId, cancellationToken);

        if (reservation.Status == ReservationStatus.NoShow)
        {
            return;
        }

        reservation.CheckOut();
    }
}