using Hotel.Application.Customers.TransferObjects;
using MediatR;

namespace Hotel.Application.Customers.Queries;

public record GetCustomersQuery() : IRequest<IReadOnlyCollection<CustomerListDto>>;