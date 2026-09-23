using Hotel.Auth.Application.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Auth.Lambda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
