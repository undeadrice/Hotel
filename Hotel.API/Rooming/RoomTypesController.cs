using Hotel.Application.Rooming.Commands;
using Hotel.Application.Rooming.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Rooming;

[ApiController]
[Route("api/[controller]")]
public class RoomTypesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRoomTypes()
    {
        var result = await mediator.Send(new GetRoomTypesQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoomTypeById(Guid id)
    {
        var result = await mediator.Send(new GetRoomTypeByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoomType(CreateRoomTypeCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRoomType(UpdateRoomTypeCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}