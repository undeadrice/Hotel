using Hotel.Application.FiscalAccounting.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.FiscalAccounting;

[ApiController]
[Route("api/[controller]")]
public class FolioItemsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateFolioItem(CreateFolioItemCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}