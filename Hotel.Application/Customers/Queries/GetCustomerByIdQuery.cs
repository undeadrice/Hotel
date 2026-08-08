using Hotel.Application.Customers.TransferObjects;
using MediatR;

namespace Hotel.Application.Customers.Queries;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto>;