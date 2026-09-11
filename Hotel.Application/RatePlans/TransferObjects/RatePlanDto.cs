namespace Hotel.Application.RatePlans.TransferObjects;

public record RatePlanDto(
    Guid Id,
    string Name,
    string TransactionCode,
    DateOnly StartDate,
    DateOnly EndDate,
    List<RatePlanRoomDto> Rooms);

public record RatePlanRoomDto(
    string RoomType,
    decimal Price);