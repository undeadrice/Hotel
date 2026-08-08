namespace Hotel.API.Customers.Responses;

public record CustomerListResponse(
    Guid Id,
    string FullName,
    string Phone,
    string Email,
    string DocumentNumber);