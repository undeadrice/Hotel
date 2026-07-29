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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoomById(Guid id)
    {
        var result = await mediator.Send(new GetRoomByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(CreateRoomCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRoom(UpdateRoomCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("status")]
    public async Task<IActionResult> ChangeRoomStatus(ChangeRoomStatusCommand command)
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

    [HttpGet("types")]
    public async Task<IActionResult> GetRoomTypes()
    {
        var result = await mediator.Send(new GetRoomTypesQuery());
        return Ok(result);
    }

    [HttpGet("types/{id:guid}")]
    public async Task<IActionResult> GetRoomTypeById(Guid id)
    {
        var result = await mediator.Send(new GetRoomTypeByIdQuery(id));
        return Ok(result);
    }

    [HttpPost("types")]
    public async Task<IActionResult> CreateRoomType(CreateRoomTypeCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("types")]
    public async Task<IActionResult> UpdateRoomType(UpdateRoomTypeCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}