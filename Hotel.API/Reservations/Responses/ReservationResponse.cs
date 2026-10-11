namespace Hotel.API.Reservations.Responses;

public record ReservationResponse(
    Guid Id,
    Guid CreatorId,
    Guid RoomId,
    Guid RatePlanId,
    DateTime StartDate,
    DateTime EndDate,
    DateTime? ArrivalTime,
    DateTime CreatedAt,
    List<Guid> GuestIds);
