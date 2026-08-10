using Hotel.Domain.Guests;

namespace Hotel.Domain.Guests.Services;

public interface IGuestRepository
{
    Task Add(Guest guest, CancellationToken token = default);

    Task Update(Guest guest, CancellationToken token = default);

    Task<Guest> GetById(Guid id, CancellationToken token = default);

    Task<Guest?> FindById(Guid id, CancellationToken token = default);

    Task<IReadOnlyCollection<Guest>> Search(
        string? name,
        string? phone,
        string? email,
        string? documentNumber,
        CancellationToken token = default);
}
