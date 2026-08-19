using Hotel.Domain.Reservations.Enums;

namespace Hotel.Application.Reservations.TransferObjects;

public record ReservationDto(
    Guid Id,
    Guid CreatorId,
    Guid RoomId,
    Guid RatePlanId,
    string CycleIdentifier,
    DateOnly StartDate,
    DateOnly EndDate,
    DateTime? ArrivalTime,
    DateTime CreatedAt,
    ReservationStatus Status,
    List<Guid> GuestIds);