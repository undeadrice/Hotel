using Hotel.Application.Pipeline;
using Hotel.Application.Reservations.TransferObjects;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Reservations.Queries;

[CheckPermission(Permission.ReservationView)]
public record GetReservationsQuery : IRequest<IReadOnlyCollection<ReservationListDto>>;
