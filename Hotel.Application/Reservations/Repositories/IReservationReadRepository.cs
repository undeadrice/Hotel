using Hotel.Application.Reservations.TransferObjects;

namespace Hotel.Application.Reservations.Repositories;

public interface IReservationReadRepository
{
    Task<IReadOnlyCollection<ReservationListDto>> GetAll(CancellationToken cancellationToken);

    Task<ReservationDto> GetById(Guid id, CancellationToken cancellationToken);
}