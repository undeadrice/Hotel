using Hotel.Application.Pipeline;

namespace Hotel.Application.Reservations.Commands;

public record CreateReservationCommand(
    Guid CreatorId,
    Guid RoomId,
    Guid RatePlanId,
    DateOnly StartDate,
    DateOnly EndDate,
    DateTime? ArrivalTime,
    List<Guid> GuestIds) : ICommand<Guid>;
