using Hotel.Application.Customers.Services;
using Hotel.Application.Customers.TransferObjects;
using MediatR;

namespace Hotel.Application.Customers.Queries;

internal class GetCustomerByIdQueryHandler(ICustomerReadRepository customerReadRepository)
    : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return await customerReadRepository.GetById(request.Id, cancellationToken);
    }
}