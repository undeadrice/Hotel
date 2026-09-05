using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Application.FiscalAccounting.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.FiscalAccounting;

[ApiController]
[Route("api/[controller]")]
public class FiscalAccountsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFiscalAccounts()
    {
        var result = await mediator.Send(new GetFiscalAccountsQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetFiscalAccountById(Guid id)
    {
        var result = await mediator.Send(new GetFiscalAccountByIdQuery(id));
        return Ok(result);
    }

    [HttpPost("{id:guid}/check-out")]
    public async Task<IActionResult> CheckOut(Guid id)
    {
        await mediator.Send(new CheckOutFiscalAccountCommand(id));
        return NoContent();
    }

    [HttpPost("{reservationId:guid}/post-room-charge")]
    public async Task<IActionResult> PostRoomCharge(Guid reservationId)
    {
        var result = await mediator.Send(new PostRoomChargeCommand(reservationId));
        return Ok(result);
    }
}
