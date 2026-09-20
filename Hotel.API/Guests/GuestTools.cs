using FluentValidation;
using Hotel.Application.Guests.Commands;
using Hotel.Application.Guests.Queries;
using Hotel.Shared.Exceptions;
using MediatR;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace Hotel.API.Guests;

[McpServerToolType]
public static class GuestTools
{
    [McpServerTool, Description("Gets all guests in the PMS system")]
    public static async Task<string> GetAllGuests(
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(
                new GetGuestsQuery(),
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

    [McpServerTool, Description("Creates a new guest in the PMS system")]
    public static async Task<string> CreateGuest(
        IMediator mediator,
        [Description("Guest's first name")] string firstName,
        [Description("Guest's last name")] string lastName,
        [Description("Guest's phone number")] string phone,
        [Description("Guest's email address")] string email,
        [Description("Guest's document number (e.g. passport or ID number)")] string documentNumber,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(
                new CreateGuestCommand(firstName, lastName, phone, email, documentNumber),
                cancellationToken);

            return JsonSerializer.Serialize(new
            {
                success = true,
                guestId = result
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
