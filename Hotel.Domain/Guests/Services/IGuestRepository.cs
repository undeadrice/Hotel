using Hotel.Domain.Guests;
using System.Linq.Expressions;

namespace Hotel.Domain.Guests.Services;

public interface IGuestRepository
{
    Task Add(Guest guest, CancellationToken token = default);

    Task Update(Guest guest, CancellationToken token = default);

    Task<Guest> GetById(Guid id, CancellationToken token = default);

    Task<Guest?> FindById(Guid id, CancellationToken token = default);

    Task<IReadOnlyCollection<Guest>> GetAll(CancellationToken token, Expression<Func<Guest, bool>>? filter = null);

    Task<IReadOnlyCollection<Guest>> Search(
        string? name,
        string? phone,
        string? email,
        string? documentNumber,
        CancellationToken token = default);
}