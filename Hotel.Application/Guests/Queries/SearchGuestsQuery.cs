using Hotel.Application.Guests.TransferObjects;
using MediatR;

namespace Hotel.Application.Guests.Queries;

public record SearchGuestsQuery(
    string? Name,
    string? Phone,
    string? Email,
    string? DocumentNumber)
    : IRequest<IReadOnlyCollection<GuestListDto>>;