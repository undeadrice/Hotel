namespace Hotel.Application.Guests.TransferObjects;

public record GuestListDto(
    Guid Id,
    string FullName,
    string Phone,
    string Email,
    string DocumentNumber);