using Hotel.Application.Transactions.Commands;
using Hotel.Application.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Transactions;

[ApiController]
[Route("api/[controller]")]
public class TransactionGroupsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTransactionGroups([FromQuery] GetTransactionGroupsQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTransactionGroupById(Guid id)
    {
        var result = await mediator.Send(new GetTransactionGroupByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransactionGroup(CreateTransactionGroupCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTransactionGroup(UpdateTransactionGroupCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("status")]
    public async Task<IActionResult> ChangeTransactionGroupStatus(ChangeTransactionGroupStatusCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}
