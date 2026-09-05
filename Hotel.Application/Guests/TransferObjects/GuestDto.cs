namespace Hotel.Application.Guests.TransferObjects;

public record GuestDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string DocumentNumber);