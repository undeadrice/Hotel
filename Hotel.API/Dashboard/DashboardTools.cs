using FluentValidation;
using Hotel.Application.Dashboard.Queries;
using Hotel.Shared.Exceptions;
using MediatR;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace Hotel.API.Dashboard;

[McpServerToolType]
public static class DashboardTools
{
    [McpServerTool, Description("Gets the dashboard view with key PMS metrics")]
    public static async Task<string> GetDashboard(
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(
                new GetDashboardQuery(),
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
}
