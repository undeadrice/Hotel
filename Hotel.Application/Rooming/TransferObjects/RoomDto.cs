using Hotel.Domain.Rooming.Enums;

namespace Hotel.Application.Rooming.TransferObjects;

public record RoomDto(
    Guid Id,
    string RoomNumber,
    Guid RoomTypeId,
    string RoomTypeName,
    RoomStatus Status,
    bool IsActive);