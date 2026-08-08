using Hotel.Application.Customers.Services;
using Hotel.Application.Customers.TransferObjects;
using MediatR;

namespace Hotel.Application.Customers.Queries;

internal class GetCustomersQueryHandler(ICustomerReadRepository customerReadRepository)
    : IRequestHandler<GetCustomersQuery, IReadOnlyCollection<CustomerListDto>>
{
    public async Task<IReadOnlyCollection<CustomerListDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        return await customerReadRepository.GetAll(cancellationToken);
    }
}