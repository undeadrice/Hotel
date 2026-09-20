namespace Hotel.Application.Rooming.TransferObjects;

public record RoomDto(
    Guid Id,
    string RoomNumber,
    Guid RoomTypeId,
    string RoomTypeName,
    bool IsActive);
