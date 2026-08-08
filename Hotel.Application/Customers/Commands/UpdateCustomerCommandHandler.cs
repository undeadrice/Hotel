using Hotel.Domain.Customers.Services;
using MediatR;

namespace Hotel.Application.Customers.Commands;

public class UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<UpdateCustomerCommand>
{
    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetById(request.Id, cancellationToken);
        customer.UpdateProfile(
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Email,
            request.DocumentNumber,
            request.Location);

        await customerRepository.Update(customer, cancellationToken);
    }
}