using Hotel.Domain.Customers;
using Hotel.Domain.Customers.Services;
using MediatR;

namespace Hotel.Application.Customers.Commands;

public class CreateCustomerCommandHandler(ICustomerRepository customerRepository) : IRequestHandler<CreateCustomerCommand, Guid>
{
    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Email,
            request.DocumentNumber);

        await customerRepository.Add(customer, cancellationToken);

        return customer.Id;
    }
}