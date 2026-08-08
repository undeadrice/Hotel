namespace Hotel.Application.Customers.TransferObjects;

public record CustomerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string DocumentNumber);
