namespace Hotel.Domain.Reservations.Services;

public interface IReservationCheckOutService
{
    Task CheckOut(Guid reservationId, CancellationToken cancellationToken = default);
}