using FluentAssertions;
using System.Net;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class EndOfDayTestData
{
    public static async Task RunEndOfDayAsync(HttpClient client)
    {
        var response = await client.PostAsync("/api/configurations/end-of-day", null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}