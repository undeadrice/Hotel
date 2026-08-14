using Hotel.Application.Configurations.Commands;
using Hotel.Application.Configurations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Configurations;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetConfiguration()
    {
        var result = await mediator.Send(new GetConfigurationQuery());
        return Ok(result);
    }

    [HttpGet("time-zones")]
    public async Task<IActionResult> GetServerTimeZones()
    {
        var result = await mediator.Send(new GetServerTimeZonesQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpsertConfiguration(UpsertConfigurationCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("end-of-day")]
    public async Task<IActionResult> PerformEndOfDay()
    {
        var result = await mediator.Send(new PerformEndOfDayCommand());
        return Ok(result);
    }
}