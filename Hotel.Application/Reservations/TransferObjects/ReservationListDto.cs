namespace Hotel.Application.Reservations.TransferObjects;

public record ReservationListDto(
    Guid Id,
    Guid CreatorId,
    Guid RoomId,
    Guid RatePlanId,
    DateTime StartDate,
    DateTime EndDate,
    DateTime? ArrivalTime,
    DateTime CreatedAt,
    List<Guid> GuestIds);