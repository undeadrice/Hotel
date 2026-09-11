namespace Hotel.Application.Rooming.TransferObjects;

public record RoomListDto(
    Guid Id,
    string RoomNumber,
    string RoomType);
