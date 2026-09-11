using FluentValidation;
using Hotel.Application.Rooming.Commands;
using Hotel.Application.Rooming.Queries;
using Hotel.Shared.Exceptions;
using MediatR;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace Hotel.API.Rooming;

[McpServerToolType]
public static class RoomTools
{
    [McpServerTool, Description("Gets all rooms in the PMS system")]
    public static async Task<string> GetAllRooms(
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(
                new GetRoomsQuery(),
                cancellationToken);

            return JsonSerializer.Serialize(new
            {
                success = true,
                data = result
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

    [McpServerTool, Description("Creates a new room in the PMS system")]
    public static async Task<string> CreateRoom(
        IMediator mediator,
        [Description("The room number identifying the room (e.g. '101', '202')")] string roomNumber,
        [Description("The unique identifier (GUID) of the room type")] Guid roomTypeId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(
                new CreateRoomCommand(roomNumber, roomTypeId),
                cancellationToken);

            return JsonSerializer.Serialize(new
            {
                success = true,
                roomId = result
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
