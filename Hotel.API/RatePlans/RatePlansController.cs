using Hotel.Application.RatePlans.Commands;
using Hotel.Application.RatePlans.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.RatePlans;

[ApiController]
[Route("api/[controller]")]
public class RatePlansController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRatePlans()
    {
        var result = await mediator.Send(new GetRatePlansQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRatePlanById(Guid id)
    {
        var result = await mediator.Send(new GetRatePlanByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRatePlan(CreateRatePlanCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRatePlan(UpdateRatePlanCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}