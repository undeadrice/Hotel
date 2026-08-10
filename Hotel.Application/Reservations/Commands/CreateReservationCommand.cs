using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Reservations.Commands;

[CheckPermission(Permission.ReservationCreate)]
public record CreateReservationCommand(
    Guid CreatorId,
    Guid RoomId,
    Guid RatePlanId,
    DateOnly StartDate,
    DateOnly EndDate,
    DateTime? ArrivalTime,
    List<Guid> GuestIds) : ICommand<Guid>;
