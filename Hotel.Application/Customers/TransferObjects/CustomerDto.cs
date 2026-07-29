using Hotel.Domain.Customers;

namespace Hotel.Application.Customers.TransferObjects;

public record CustomerDto(Guid Id, CustomerLocation Location);
