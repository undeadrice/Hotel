using Hotel.API.Customers.Mappings;
using Hotel.Application.Customers.Commands;
using Hotel.Application.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Customers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CreateCustomerCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, UpdateCustomerCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Id mismatch.");
        }

        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var result = await mediator.Send(new GetCustomersQuery());
        var response = result.Select(r => r.MapToCustomerListResponse());
        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCustomers(
        [FromQuery] string? name,
        [FromQuery] string? phone,
        [FromQuery] string? email,
        [FromQuery] string? documentNumber)
    {
        var result = await mediator.Send(new SearchCustomersQuery(name, phone, email, documentNumber));
        var response = result.Select(r => r.MapToCustomerListResponse());
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var result = await mediator.Send(new GetCustomerByIdQuery(id));
        return Ok(result.MapToCustomerResponse());
    }
}