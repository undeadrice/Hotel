using Hotel.Application.Transactions.Commands;
using Hotel.Application.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Transactions;

[ApiController]
[Route("api/[controller]")]
public class TransactionCodesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTransactionCodes(
        [FromQuery] Guid? transactionGroupId,
        [FromQuery] bool? isActive)
    {
        var result = await mediator.Send(new GetTransactionCodesQuery(transactionGroupId, isActive));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTransactionCodeById(Guid id)
    {
        var result = await mediator.Send(new GetTransactionCodeByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransactionCode(CreateTransactionCodeCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTransactionCode(UpdateTransactionCodeCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("status")]
    public async Task<IActionResult> ChangeTransactionCodeStatus(ChangeTransactionCodeStatusCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}
