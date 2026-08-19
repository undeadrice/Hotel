using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Reservations.Commands;

[CheckPermission(Permission.ReservationEdit)]
public record CheckInReservationCommand(Guid ReservationId) : ICommand;