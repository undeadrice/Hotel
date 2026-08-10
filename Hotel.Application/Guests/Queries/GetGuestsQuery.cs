using Hotel.Application.Guests.TransferObjects;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Guests.Queries;

[CheckPermission(Permission.GuestView)]
public record GetGuestsQuery() : IRequest<IReadOnlyCollection<GuestListDto>>;
