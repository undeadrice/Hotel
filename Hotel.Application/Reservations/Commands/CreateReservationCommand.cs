using Hotel.Application.Pipeline;

namespace Hotel.Application.Reservations.Commands;

public record CreateReservationCommand(
    Guid CreatorId,
    Guid RoomId,
    DateTime StartDate,
    DateTime EndDate,
    List<Guid> GuestIds) : ICommand<Guid>;