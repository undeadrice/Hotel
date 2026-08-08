namespace Hotel.Application.Customers.TransferObjects;

public record CustomerListDto(
    Guid Id,
    string FullName,
    string Phone,
    string Email,
    string DocumentNumber);