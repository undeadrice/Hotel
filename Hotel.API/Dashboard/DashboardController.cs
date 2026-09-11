using Hotel.Application.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Dashboard;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await mediator.Send(new GetDashboardQuery());
        return Ok(result);
    }
}