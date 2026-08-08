namespace Hotel.API.Customers.Responses;

public record CustomerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string DocumentNumber);
