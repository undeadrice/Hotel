namespace Hotel.API.Reservations.Responses;

public record ReservationResponse(
    Guid Id,
    Guid CreatorId,
    Guid RoomId,
    DateTime StartDate,
    DateTime EndDate,
    DateTime CreatedAt,
    List<Guid> GuestIds);