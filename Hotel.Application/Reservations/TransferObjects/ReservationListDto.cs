namespace Hotel.Application.Reservations.TransferObjects;

public record ReservationListDto(
    Guid Id,
    string CycleIdentifier,
    string RoomName,
    string RatePlanName,
    string CreatorName,
    DateOnly StartDate,
    DateOnly EndDate,
    DateTime? ArrivalTime,
    DateTime CreatedAt,
    int GuestCount);
