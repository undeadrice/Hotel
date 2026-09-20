using Hotel.Application.NumberCycles.Commands;
using Hotel.Application.NumberCycles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.NumberCycles;

[ApiController]
[Route("api/[controller]")]
public class NumberCyclesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetNumberCycles()
    {
        var result = await mediator.Send(new GetNumberCyclesQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetNumberCycleById(Guid id)
    {
        var result = await mediator.Send(new GetNumberCycleByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNumberCycle(CreateNumberCycleCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteNumberCycle(Guid id)
    {
        await mediator.Send(new DeleteNumberCycleCommand(id));
        return NoContent();
    }
}