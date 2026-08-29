using Hotel.Application.Rooming.Commands;
using Hotel.Application.Rooming.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Rooming;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        var result = await mediator.Send(new GetRoomsQuery());
        return Ok(result);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableRooms(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate)
    {
        var result = await mediator.Send(new GetAvailableRoomsQuery(startDate, endDate));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoomById(Guid id)
    {
        var result = await mediator.Send(new GetRoomByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(CreateRoomCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRoom(UpdateRoomCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("deactivate")]
    public async Task<IActionResult> DeactivateRoom(DeactivateRoomCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}