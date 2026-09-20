using FluentAssertions;
using Hotel.Application.NumberCycles.Commands;
using Hotel.Domain.NumberCycles.Enums;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class NumberCycleTestData
{
    public static async Task<Guid> CreateNumberCycleAsync(
        HttpClient client,
        NumberCycleTopic topic = NumberCycleTopic.Reservation,
        string prefix = "RES",
        int startIndex = 1)
    {
        var response = await client.PostAsJsonAsync(
            "/api/numbercycles",
            new CreateNumberCycleCommand(topic, prefix, startIndex));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}