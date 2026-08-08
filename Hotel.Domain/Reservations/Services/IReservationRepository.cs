using Hotel.Domain.Reservations.Entities;
using System.Linq.Expressions;

namespace Hotel.Domain.Reservations.Services;

public interface IReservationRepository
{
    Task Add(Reservation reservation, CancellationToken token = default);

    Task Update(Reservation reservation, CancellationToken token = default);

    Task<Reservation?> FindById(Guid id, CancellationToken token = default);

    Task<Reservation> GetById(Guid id, CancellationToken token = default);

    Task<IReadOnlyCollection<Reservation>> GetAll(CancellationToken token, Expression<Func<Reservation, bool>>? filter = null);
}