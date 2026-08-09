using Hotel.Application.Reservations.Commands;
using Hotel.Application.Reservations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Reservations;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetReservations()
    {
        var result = await mediator.Send(new GetReservationsQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetReservationById(Guid id)
    {
        var result = await mediator.Send(new GetReservationByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(CreateReservationCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
