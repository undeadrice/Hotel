namespace Hotel.Application.Reservations.TransferObjects;

public record ReservationListDto(
    Guid Id,
    string RoomName,
    string RatePlanName,
    string CreatorName,
    DateTime StartDate,
    DateTime EndDate,
    DateTime? ArrivalTime,
    DateTime CreatedAt,
    int GuestCount);
