using Hotel.API.Guests.Mappings;
using Hotel.Application.Guests.Commands;
using Hotel.Application.Guests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Guests;

[ApiController]
[Route("api/[controller]")]
public class GuestsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateGuest(CreateGuestCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateGuest(UpdateGuestCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetGuests()
    {
        var result = await mediator.Send(new GetGuestsQuery());
        var response = result.Select(r => r.MapToGuestListResponse());
        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchGuests(
        [FromQuery] string? name,
        [FromQuery] string? phone,
        [FromQuery] string? email,
        [FromQuery] string? documentNumber)
    {
        var result = await mediator.Send(new SearchGuestsQuery(name, phone, email, documentNumber));
        var response = result.Select(r => r.MapToGuestListResponse());
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetGuestById(Guid id)
    {
        var result = await mediator.Send(new GetGuestByIdQuery(id));
        return Ok(result.MapToGuestResponse());
    }
}