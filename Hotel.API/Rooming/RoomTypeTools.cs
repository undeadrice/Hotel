using FluentValidation;
using Hotel.Application.Rooming.Commands;
using Hotel.Shared.Exceptions;
using MediatR;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace Hotel.API.Rooming;

[McpServerToolType]
public static class RoomTypeTools
{
    [McpServerTool, Description("Creates new room type in the PMS system")]
    public static async Task<string> CreateRoomType(
        IMediator mediator,
        [Description("Name defining the type of the room by the type of the bed. eg (King, Double, Standard")] string name,
        [Description("Optional description of the room type")] string? description = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(
                new CreateRoomTypeCommand(name, description),
                cancellationToken);

            return JsonSerializer.Serialize(new
            {
                success = true,
                roomTypeId = result
            });
        }
        catch (ValidationException ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = "validation_failed",
                details = ex.Errors.Select(e => e.ErrorMessage)
            });
        }
        catch (DomainException ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = "domain_error",
                details = ex.Message
            });
        }
    }
}

