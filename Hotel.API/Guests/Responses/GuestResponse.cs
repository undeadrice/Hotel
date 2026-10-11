namespace Hotel.API.Guests.Responses;

public record GuestResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string DocumentNumber);