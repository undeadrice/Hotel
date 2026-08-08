using Hotel.Application.Guests.TransferObjects;

namespace Hotel.Application.Guests.Services;

public interface IGuestReadRepository
{
    Task<IReadOnlyCollection<GuestListDto>> GetAll(CancellationToken cancellationToken);

    Task<GuestDto> GetById(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<GuestListDto>> Search(
        string? name,
        string? phone,
        string? email,
        string? documentNumber,
        CancellationToken cancellationToken);
}