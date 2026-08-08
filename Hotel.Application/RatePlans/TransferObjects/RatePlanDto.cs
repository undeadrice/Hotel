namespace Hotel.Application.RatePlans.TransferObjects;

public record RatePlanDto(
    Guid Id,
    string Name,
    Guid TransactionCodeId,
    DateOnly StartDate,
    DateOnly EndDate,
    List<RatePlanRoomDto> Rooms);

public record RatePlanRoomDto(
    Guid RoomTypeId,
    decimal Price);