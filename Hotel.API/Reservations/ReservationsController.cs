using Hotel.Application.Reservations.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Reservations;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateReservation(CreateReservationCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}