using FluentAssertions;
using Hotel.Application.RatePlans.Commands;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class RatePlanTestData
{
    public static async Task<Guid> CreateRatePlanAsync(
        HttpClient client,
        Guid transactionCodeId,
        Guid roomTypeId,
        string name = "Standard",
        decimal price = 100m,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        var start = startDate ?? RatePlanDates.ValidStartDate;
        var end = endDate ?? RatePlanDates.ValidEndDate;

        var response = await client.PostAsJsonAsync(
            "/api/rateplans",
            new CreateRatePlanCommand(
                name,
                transactionCodeId,
                start,
                end,
                [new CreateRatePlanRoomCommand(roomTypeId, price)]));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}