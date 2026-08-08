using Hotel.Application.Guests.TransferObjects;
using MediatR;

namespace Hotel.Application.Guests.Queries;

public record GetGuestsQuery() : IRequest<IReadOnlyCollection<GuestListDto>>;