using Hotel.API.Customers.Responses;
using Hotel.Application.Customers.TransferObjects;

namespace Hotel.API.Customers.Mappings;

public static class CustomerMappingExtensions
{
    public static CustomerResponse MapToCustomerResponse(this CustomerDto dto) =>
        new CustomerResponse(dto.Id, dto.Location.ToString());
}
