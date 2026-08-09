using Hotel.Application.Reservations.TransferObjects;

namespace Hotel.Application.Reservations.Services;

public interface IReservationReadRepository
{
    Task<IReadOnlyCollection<ReservationListDto>> GetAll(CancellationToken cancellationToken);

    Task<ReservationDto> GetById(Guid id, CancellationToken cancellationToken);
}