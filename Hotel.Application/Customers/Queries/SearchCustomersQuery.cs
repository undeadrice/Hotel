using Hotel.Application.Customers.TransferObjects;
using MediatR;

namespace Hotel.Application.Customers.Queries;

public record SearchCustomersQuery(
    string? Name,
    string? Phone,
    string? Email,
    string? DocumentNumber)
    : IRequest<IReadOnlyCollection<CustomerListDto>>;