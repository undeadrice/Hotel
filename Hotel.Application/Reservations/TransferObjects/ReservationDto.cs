namespace Hotel.Application.Reservations.TransferObjects;

public record ReservationDto(
    Guid Id,
    Guid CreatorId,
    Guid RoomId,
    Guid RatePlanId,
    DateOnly StartDate,
    DateOnly EndDate,
    DateTime? ArrivalTime,
    DateTime CreatedAt,
    List<Guid> GuestIds);
