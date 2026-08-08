namespace Hotel.Application.Rooming.TransferObjects;

public record RoomTypeDto(
    Guid Id,
    string Name,
    string? Description);
