namespace Hotel.API.Guests.Responses;

public record GuestListResponse(
    Guid Id,
    string FullName,
    string Phone,
    string Email,
    string DocumentNumber);